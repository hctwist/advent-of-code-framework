namespace AdventOfCode.Framework.Tools;

/// <summary>
/// A readonly matrix.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public interface IReadOnlyMatrix<T> : IEnumerable<MatrixCell<T>>
{
    /// <summary>
    /// Gets the row count of the matrix.
    /// </summary>
    int Rows { get; }

    /// <summary>
    /// Gets the column count of the matrix.
    /// </summary>
    int Columns { get; }

    /// <summary>
    /// Gets the total element count of the matrix.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets a cell from the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    MatrixCell<T> this[int row, int column] { get; }

    /// <summary>
    /// Gets a cell from the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    MatrixCell<T> this[Index row, Index column] { get; }

    /// <summary>
    /// Determines whether a row and column index are within bounds of the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>True if the indices are in bounds, false otherwise.</returns>
    bool IsWithinBounds(int row, int column);

    /// <summary>
    /// Gets a cell from the matrix, or null if the indices are out of bounds.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <returns>The matrix cell, or null if the indices are out of bounds.</returns>
    MatrixCell<T>? GetOrDefault(int row, int column);

    /// <summary>
    /// Gets cells in a single row of the matrix.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <returns>The cells in row <paramref name="row"/>.</returns>
    IEnumerable<MatrixCell<T>> GetRow(int row);

    /// <summary>
    /// Gets cells in a single column of the matrix.
    /// </summary>
    /// <param name="column">The column index.</param>
    /// <returns>The cells in column <paramref name="column"/>.</returns>
    IEnumerable<MatrixCell<T>> GetColumn(int column);
}