using System.Drawing;

namespace EvilGiraffes.Collections;
/// <summary>
/// A wrapper for a 2 dimensional matrix of any type. 
/// </summary>
public class Matrix2D<T>
{
    public T[,] Value {get; private set;}
    public int Width { get; init; }
    public int Length { get; init; }
    private bool _initalized = false;
    
    public Matrix2D(int x, int y)
    {
        Width = x;
        Length = y;
        Value = new T[Length, Width];
    }
    public Matrix2D(int x, int y, T defaultValue): this(x, y)
    {
        Initalize(defaultValue);
    }
    public Matrix2D(Point point): this(point.X, point.Y) {}
    public Matrix2D(Point point, T defaultValue): this(point.X, point.Y, defaultValue) {}
    /// <summary>
    /// Get the entire row.
    /// </summary>
    /// <param name="y">Y Coordinate of the matrix to get the values from.</param>
    /// <returns>A new array with the values from the row.</returns>
    public T[] Row(int y)
    {
        T[] result = new T[Width];
        for (int i = 0; i < Width; i++)
        {
            result[i] = this[i, y];
        }
        return result;
    }
    /// <summary>
    /// Set the entire row. 
    /// </summary>
    /// <param name="y">Y Coordinate of the matrix to set the new values to.</param>
    /// <param name="value">The new array to set it to</param>
    /// <returns>If value has been set.</returns>
    public bool Row(int y, T[] value)
    {
        if (value.Length != Width) return false;
        for (int i = 0; i < Width; i++)
        {
            this[i, y] = value[i];
        }
        return true;
    }
    /// <summary>
    /// Get the entire column.
    /// </summary>
    /// <param name="x">Y Coordinate of the matrix to get the values from.</param>
    /// <returns>A new array with the values from the column.</returns>
    public T[] Column(int x)
    {
        T[] result = new T[Length];
        for (int i = 0; i < Length; i++)
        {
            result[i] = this[x, i];
        }
        return result;
    }
    /// <summary>
    /// Set the entire column. 
    /// </summary>
    /// <param name="x">Y Coordinate of the matrix to set the new values to.</param>
    /// <param name="value">The new array to set it to.</param>
    /// <returns>If value has been set.</returns>
    public bool Column(int x, T[] value)
    {
        if (value.Length != Length) return false;
        for (int i = 0; i < Length; i++)
        {
            this[x, i] = value[i];
        }
        return true;
    }
    /// <summary>
    /// Will return or set values based on index's 
    /// </summary>
    /// <value>Accesses the matrix via index's.</value>
    public T this[int x, int y]
    {
        get => Value[y, x]; 
        set => Value[y, x] = value;
    }
    /// <summary>
    /// Will return or set values based on Point 
    /// </summary>
    /// <value>Accesses the matrix via index's.</value>
    public T this[Point point]
    {
        get => this[point.X, point.Y];
        set => this[point.X, point.Y] = value;
    }
    /// <summary>
    /// Initalizes the Matrix with default values.
    /// </summary>
    /// <param name="defaultValue"> The value to initalize</param>
    /// <exception>MatrixInitializedException</exception>
    public void Initalize(T defaultValue)
    {
        if (_initalized is true) throw new Errors.MatrixInitializedException();
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                this[x, y] = defaultValue;
            }
        }
        _initalized = true;
    }
}