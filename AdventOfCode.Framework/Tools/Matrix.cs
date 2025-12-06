using System.Collections;

namespace AdventOfCode.Framework.Tools;

/// <summary>
/// A matrix.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public class Matrix<T> : IReadOnlyMatrix<T>
{
    private readonly T[,] data;

    /// <summary>
    /// Creates a new <see cref="Matrix{T}"/>
    /// </summary>
    /// <param name="rows">The number of rows.</param>
    /// <param name="columns">The number of columns.</param>
    /// <param name="value">The value to fill the matrix with.</param>
    public Matrix(int rows, int columns, T value)
    {
        data = new T[rows, columns];
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                data[row, column] = value;
            }
        }
    }

    /// <summary>
    /// Creates a new <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="data">The data. This will be copied.</param>
    public Matrix(T[,] data) : this(data, true)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="matrix">The source matrix. This will be copied.</param>
    public Matrix(IReadOnlyMatrix<T> matrix)
    {
        data = new T[matrix.Rows, matrix.Columns];
        for (var row = 0; row < matrix.Rows; row++)
        {
            for (var column = 0; column < matrix.Columns; column++)
            {
                data[row, column] = matrix[row, column];
            }
        }
    }

    private Matrix(T[,] data, bool copy)
    {
        if (copy)
        {
            this.data = new T[data.GetLength(0), data.GetLength(1)];
            Array.Copy(data, this.data, data.Length);
        }
        else
        {
            this.data = data;
        }
    }

    /// <inheritdoc/>
    public int Rows => data.GetLength(0);

    /// <inheritdoc/>
    public int Columns => data.GetLength(1);

    /// <inheritdoc/>
    public int Count => data.Length;

    /// <inheritdoc/>
    public MatrixCell<T> this[int row, int column] => new(row, column, data[row, column], this);

    /// <inheritdoc/>
    public MatrixCell<T> this[Index row, Index column] => this[row.GetOffset(Rows), column.GetOffset(Columns)];

    /// <summary>
    /// Sets a value in the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <param name="value">The value to set.</param>
    public void Set(int row, int column, T value)
    {
        data[row, column] = value;
    }

    /// <inheritdoc/>
    public bool IsWithinBounds(int row, int column)
    {
        return (0 <= row && row < Rows) && (0 <= column && column < Columns);
    }

    /// <inheritdoc/>
    public MatrixCell<T>? GetOrDefault(int row, int column)
    {
        return IsWithinBounds(row, column) ? this[row, column] : null;
    }

    /// <inheritdoc/>
    public IEnumerable<MatrixCell<T>> GetRow(int row)
    {
        for (var column = 0; column < Columns; column++)
        {
            yield return this[row, column];
        }
    }

    /// <inheritdoc/>
    public IEnumerable<MatrixCell<T>> GetColumn(int column)
    {
        for (var row = 0; row < Rows; row++)
        {
            yield return this[row, column];
        }
    }

    /// <inheritdoc />
    public IEnumerator<MatrixCell<T>> GetEnumerator()
    {
        return new Enumerator(this);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator : IEnumerator<MatrixCell<T>>
    {
        private readonly Matrix<T> matrix;

        private int row;
        private int column;

        public Enumerator(Matrix<T> matrix)
        {
            this.matrix = matrix;
            row = 0;
            column = -1;
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            // Try and move right
            column++;

            if (!matrix.IsWithinBounds(row, column))
            {
                // Try and move down
                row++;
                column = 0;
            }

            return matrix.IsWithinBounds(row, column);
        }

        /// <inheritdoc />
        public MatrixCell<T> Current => matrix[row, column];

        /// <inheritdoc />
        object? IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}