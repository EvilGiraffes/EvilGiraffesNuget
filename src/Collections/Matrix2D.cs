using System.Collections;
using System.Drawing;

using EvilGiraffes.Errors;

namespace EvilGiraffes.Collections;
/// <summary>
/// A wrapper for a 2 dimensional matrix of any type. Implements IEnumerable. 
/// </summary>
public class Matrix2D<T>: IEnumerable<T>
{
    /// <summary>
    /// The basic 2D array which this class wraps.
    /// </summary>
    /// <value>returns T[,].</value>
    public T[,] Value {get; private set;}
    /// <summary>
    /// The width (rows) of the matrix.
    /// </summary>
    /// <value>returns int.</value>
    public int Width { get; init; }
    /// <summary>
    /// The length (columns) of the matrix.
    /// </summary>
    /// <value>returns int.</value>
    public int Length { get; init; }
    /// <summary>
    /// The count of all values in the matrix, Lenght * Width.
    /// </summary>
    /// <value>returns int.</value>
    public int Count { get; init; }
    private bool _initalized = false;
    /// <summary>
    /// Constructs a new instance of a matrix.
    /// </summary>
    /// <param name="x">Width of the matrix.</param>
    /// <param name="y">Length of the matrix.</param>
    public Matrix2D(int x, int y)
    {
        Width = x;
        Length = y;
        Count = Length * Width;
        Value = new T[Length, Width];
    }
    /// <summary>
    /// Constructs a new instance of a matrix.
    /// </summary>
    /// <param name="x">Width of the matrix.</param>
    /// <param name="y">Length of the matrix.</param>
    /// <param name="defaultValue">A default value to instanciate it with.</param>
    public Matrix2D(int x, int y, T defaultValue): this(x, y)
    {
        Initalize(defaultValue);
    }
    /// <summary>
    /// Constructs a new instance of a matrix.
    /// </summary>
    /// <param name="point">A point struct containing X (Width) and Y (Length) values.</param>
    public Matrix2D(Point point): this(point.X, point.Y) {}
    /// <summary>
    /// Constructs a new instance of a matrix.
    /// </summary>
    /// <param name="point">A point struct containing X (Width) and Y (Length) values.</param>
    /// <param name="defaultValue">A default value to instanciate it with.</param>
    /// <returns></returns>
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
    /// <param name="value">The new array to set it to.</param>
    /// <returns>TRUE if value has been set, FALSE if value is unchanged.</returns>
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
    /// <param name="x">X Coordinate of the matrix to get the values from.</param>
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
    /// <param name="x">X Coordinate of the matrix to set the new values to.</param>
    /// <param name="value">The new array to set it to.</param>
    /// <returns>TRUE if value has been set, FALSE if value is unchanged.</returns>
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
    /// Will return or set values based on index's.
    /// </summary>
    /// <value>return T type.</value>
    public T this[int x, int y]
    {
        get => Value[y, x]; 
        set => Value[y, x] = value;
    }
    /// <summary>
    /// Will return or set values based on Point.
    /// </summary>
    /// <value>return T type.</value>
    public T this[Point point]
    {
        get => this[point.X, point.Y];
        set => this[point.X, point.Y] = value;
    }
    /// <summary>
    /// Initalizes the Matrix with default values.
    /// </summary>
    /// <param name="defaultValue"> The value to initalize.</param>
    /// <exception>MatrixInitializedException</exception>
    public void Initalize(T defaultValue)
    {
        if (_initalized is true) throw new MatrixInitializedException();
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                this[x, y] = defaultValue;
            }
        }
        _initalized = true;
    }
    /// <summary>
    /// Will give an enumerator which will give index's of the entire array instead of directly enumerating.
    /// </summary>
    /// <returns>IEnumerator of type Point.</returns>
    public IEnumerator<Point> GetIndexEnumerator()
    {
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return new Point(x, y);
            }
        }
    }
    /// <summary>
    /// Will give an Enumerator to enumerate over values inside matrix.
    /// </summary>
    /// <returns>IEnumerator of type T.</returns>
    public IEnumerator<T> GetEnumerator() 
    {
        foreach (T val in Value) yield return val;
    }
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}