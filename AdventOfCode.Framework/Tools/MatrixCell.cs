namespace AdventOfCode.Framework.Tools;

/// <summary>
/// Single cell of a matrix.
/// </summary>
/// <typeparam name="T">The cell's value type.</typeparam>
public readonly struct MatrixCell<T>
{
    /// <summary>
    /// Gets the row index of the cell.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Gets the column index of the cell.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Gets the cell value.
    /// </summary>
    public T Value { get; }

    private readonly ImmutableMatrix<T> sourceMatrix;

    internal MatrixCell(
        int row,
        int column,
        T value,
        ImmutableMatrix<T> sourceMatrix)
    {
        Row = row;
        Column = column;
        Value = value;

        this.sourceMatrix = sourceMatrix;
    }

    /// <summary>
    /// Gets the cell to the top of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? Top => Offset(-1, 0);

    /// <summary>
    /// Gets the cell to the top right of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? TopRight => Offset(-1, 1);

    /// <summary>
    /// Gets the cell to the right of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? Right => Offset(0, 1);

    /// <summary>
    /// Gets the cell to the bottom right of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? BottomRight => Offset(1, 1);

    /// <summary>
    /// Gets the cell to the bottom of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? Bottom => Offset(1, 0);

    /// <summary>
    /// Gets the cell to the bottom left of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? BottomLeft => Offset(1, -1);

    /// <summary>
    /// Gets the cell to the left of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? Left => Offset(0, -1);

    /// <summary>
    /// Gets the cell to the top left of this one in the matrix, or null if it is out of bounds.
    /// </summary>
    public MatrixCell<T>? TopLeft => Offset(-1, -1);

    /// <summary>
    /// Gets a cell relative to this one in the matrix, or null if it is out of bounds.
    /// </summary>
    /// <param name="rowOffset">The row offset.</param>
    /// <param name="columnOffset">The column offset.</param>
    /// <returns>The relative cell, or null if it is out of bounds.</returns>
    public MatrixCell<T>? Offset(int rowOffset, int columnOffset)
    {
        return sourceMatrix.GetOrDefault(Row + rowOffset, Column + columnOffset);
    }

    /// <summary>
    /// Gets the four adjacent cells to this one in the matrix (top, right, bottom, left).
    /// </summary>
    /// <remarks>Note that this enumerable can have less than four elements if adjacent cells are out of bounds.</remarks>
    /// <returns>The adjacent cells.</returns>
    public IEnumerable<MatrixCell<T>> Adjacent4()
    {
        if (Top is MatrixCell<T> top)
        {
            yield return top;
        }

        if (Right is MatrixCell<T> right)
        {
            yield return right;
        }

        if (Bottom is MatrixCell<T> bottom)
        {
            yield return bottom;
        }

        if (Left is MatrixCell<T> left)
        {
            yield return left;
        }
    }

    /// <summary>
    /// Gets the eight adjacent cells to this one in the matrix (top, top right, right, bottom right, bottom, bottom left, left, top left).
    /// </summary>
    /// <remarks>Note that this enumerable can have less than four elements if adjacent cells are out of bounds.</remarks>
    /// <returns>The adjacent cells.</returns>
    public IEnumerable<MatrixCell<T>> Adjacent8()
    {
        if (Top is MatrixCell<T> top)
        {
            yield return top;
        }

        if (TopRight is MatrixCell<T> topRight)
        {
            yield return topRight;
        }

        if (Right is MatrixCell<T> right)
        {
            yield return right;
        }

        if (BottomRight is MatrixCell<T> bottomRight)
        {
            yield return bottomRight;
        }

        if (Bottom is MatrixCell<T> bottom)
        {
            yield return bottom;
        }

        if (BottomLeft is MatrixCell<T> bottomLeft)
        {
            yield return bottomLeft;
        }

        if (Left is MatrixCell<T> left)
        {
            yield return left;
        }

        if (TopLeft is MatrixCell<T> topLeft)
        {
            yield return topLeft;
        }
    }
}