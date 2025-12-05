using System.Collections;

namespace AdventOfCode.Framework.Tools;

// TODO Allow mutable matrix that you can modify elements in. Have IReadOnlyMatrix for the readonly part
/// <summary>
/// An immutable matrix.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public class ImmutableMatrix<T> : IEnumerable<MatrixCell<T>>
{
    private readonly T[,] data;

    /// <summary>
    /// Creates a new <see cref="ImmutableMatrix{T}"/>.
    /// </summary>
    /// <param name="data">The data.</param>
    public ImmutableMatrix(T[,] data) : this(data, true)
    {
    }

    private ImmutableMatrix(T[,] data, bool copy)
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

    /// <summary>
    /// Gets the row count of the matrix.
    /// </summary>
    public int Rows => data.GetLength(0);

    /// <summary>
    /// Gets the column count of the matrix.
    /// </summary>
    public int Columns => data.GetLength(1);

    /// <summary>
    /// Gets the total element count of the matrix.
    /// </summary>
    public int Count => data.Length;

    /// <summary>
    /// Gets a cell from the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    public MatrixCell<T> this[int row, int column] => new(row, column, data[row, column], this);

    /// <summary>
    /// Determines whether a row and column index are within bounds of the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>True if the indices are in bounds, false otherwise.</returns>
    public bool IsWithinBounds(int row, int column)
    {
        return (0 <= row && row < Rows) && (0 <= column && column < Columns);
    }

    /// <summary>
    /// Gets a cell from the matrix, or null if the indices are out of bounds.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>The matrix cell, or null if the indices are out of bounds.</returns>
    public MatrixCell<T>? GetOrDefault(int row, int column)
    {
        return IsWithinBounds(row, column) ? this[row, column] : null;
    }

    /// <summary>
    /// Gets cells in a single row of the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <returns>The cells in row <paramref name="row"/>.</returns>
    public IEnumerable<MatrixCell<T>> GetRow(int row)
    {
        for (var column = 0; column < Columns; column++)
        {
            yield return this[row, column];
        }
    }

    /// <summary>
    /// Gets cells in a single column of the matrix.
    /// </summary>
    /// <param name="column">The column index.</param>
    /// <returns>The cells in column <paramref name="column"/>.</returns>
    public IEnumerable<MatrixCell<T>> GetColumn(int column)
    {
        for (var row = 0; row < Rows; row++)
        {
            yield return this[row, column];
        }
    }

    /// <summary>
    /// Creates a new matrix with each element selected from the original.
    /// </summary>
    /// <param name="selector">The selector.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The resulting matrix.</returns>
    public ImmutableMatrix<TResult> Select<TResult>(Func<T, TResult> selector)
    {
        var newData = new TResult[Rows, Columns];

        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                newData[r, c] = selector(data[r, c]);
            }
        }

        return new ImmutableMatrix<TResult>(newData, false);
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
        private readonly ImmutableMatrix<T> matrix;

        private int row;
        private int column;

        public Enumerator(ImmutableMatrix<T> matrix)
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