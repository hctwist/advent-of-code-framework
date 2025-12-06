using System.Collections;

namespace AdventOfCode.Framework.Tools;

public static class MatrixExtensions
{
    extension<T>(IReadOnlyMatrix<T> matrix)
    {
        /// <summary>
        /// Creates a new matrix with each element selected from the original.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <returns>The resulting matrix.</returns>
        public IReadOnlyMatrix<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return new SelectedMatrix<T, TResult>(matrix, selector);
        }

        /// <summary>
        /// Creates a new matrix from the readonly matrix.
        /// </summary>
        /// <returns>The new matrix.</returns>
        public Matrix<T> ToMatrix()
        {
            return new Matrix<T>(matrix);
        }
    }

    private class SelectedMatrix<TSource, TResult> : IReadOnlyMatrix<TResult>
    {
        private readonly IReadOnlyMatrix<TSource> sourceMatrix;
        private readonly Func<TSource, TResult> selector;

        internal SelectedMatrix(IReadOnlyMatrix<TSource> sourceMatrix, Func<TSource, TResult> selector)
        {
            this.sourceMatrix = sourceMatrix;
            this.selector = selector;
        }

        /// <inheritdoc />
        public int Rows => sourceMatrix.Rows;

        /// <inheritdoc />
        public int Columns => sourceMatrix.Columns;

        /// <inheritdoc />
        public int Count => sourceMatrix.Count;

        /// <inheritdoc />
        public MatrixCell<TResult> this[int row, int column] => Select(sourceMatrix[row, column]);

        /// <inheritdoc />
        public MatrixCell<TResult> this[Index row, Index column] => Select(sourceMatrix[row, column]);

        /// <inheritdoc />
        public bool IsWithinBounds(int row, int column)
        {
            return sourceMatrix.IsWithinBounds(row, column);
        }

        /// <inheritdoc />
        public MatrixCell<TResult>? GetOrDefault(int row, int column)
        {
            return sourceMatrix.GetOrDefault(row, column) is MatrixCell<TSource> c ? Select(c) : null;
        }

        /// <inheritdoc />
        public IEnumerable<MatrixCell<TResult>> GetRow(int row)
        {
            return sourceMatrix.GetRow(row).Select(Select);
        }

        /// <inheritdoc />
        public IEnumerable<MatrixCell<TResult>> GetColumn(int column)
        {
            return sourceMatrix.GetColumn(column).Select(Select);
        }

        /// <inheritdoc />
        public IEnumerator<MatrixCell<TResult>> GetEnumerator()
        {
            return sourceMatrix.Select(Select).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private MatrixCell<TResult> Select(MatrixCell<TSource> sourceCell)
        {
            return new MatrixCell<TResult>(sourceCell.Row, sourceCell.Column, selector(sourceCell.Value), this);
        }
    }
}