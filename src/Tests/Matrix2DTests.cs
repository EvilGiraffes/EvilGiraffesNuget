using System.Text;
using System.Drawing;

using Xunit;
using Xunit.Abstractions;

using EvilGiraffes.Collections;
using EvilGiraffes.Errors;

namespace EvilGiraffes.Tests;

public class Matrix2DTest
{
    private readonly ITestOutputHelper _output;
    private readonly Random _random;
    private readonly string _outputSeperator;
    public Matrix2DTest(ITestOutputHelper output)
    {
        _output = output;
        _random = new();
        _outputSeperator = "\t\t";
        
    }
    [Theory]
    [MemberData(nameof(LengthWidthData))]
    public void PropertyTest(int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int count = length * width;
        _PropertyOutput(matrix, length, width, count);
        Assert.Equal(width, matrix.Width);
        Assert.Equal(length, matrix.Length);
        Assert.Equal(count, matrix.Count);
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void RowTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int y = _random.Next(length);
        int[] insertArray = _PopulateArray<int>(inserted, width);
        matrix.Row(y, insertArray);
        _ArrayOutput<int>(insertArray, matrix.Row(y));
        Assert.Equal(insertArray, matrix.Row(y));
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void ColumnTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int x = _random.Next(width);
        int[] insertArray = _PopulateArray<int>(inserted, length);
        matrix.Column(x, insertArray);
        _ArrayOutput<int>(insertArray, matrix.Column(x));
        Assert.Equal(insertArray, matrix.Column(x));
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void IndexIntTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int x = _random.Next(width);
        int y = _random.Next(length);
        _IndexOutput(inserted, length, width, x, y);
        matrix[x, y] = inserted;
        Assert.Equal(inserted, matrix [x, y]);
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void IndexPointTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        Point point = new(
            _random.Next(width),
            _random.Next(length)
        );
        _IndexOutput(inserted, length, width, point.X, point.Y);
        matrix[point] = inserted;
        Assert.Equal(inserted, matrix[point]);
    }
    [Theory]
    [MemberData(nameof(LengthWidthData))]
    public void InitializationExceptionTest(int length, int width)
    {
        Matrix2D<int> matrix = new(width, length);
        matrix.Initalize(0);
        var exception = Record.Exception(
            () => matrix.Initalize(4)
        );
        _output.WriteLine($"Expected >> {nameof(MatrixInitializedException)}{_outputSeperator}Outputted >> {exception.GetType().Name}");
        Assert.IsType<MatrixInitializedException>(exception);
    }
    public static IEnumerable<object[]> LengthWidthData()
    {
        int testAmount = 7;
        int min = 1;
        int max = 30;
        Random random = new();
        for (int i = 0; i < testAmount; i++)
        {
            yield return new object[] { random.Next(min, max), random.Next(min, max) };
        }
    }
    public static IEnumerable<object[]> InsertLengthWidthData()
    {
        int testAmount = 7;
        int min = 1;
        int max = 30;
        Random random = new();
        for (int i = 0; i < testAmount; i++)
        {
            yield return new object[] { random.Next(min, max), random.Next(min, max), random.Next(min, max) };
        }
    }
    private T[] _PopulateArray<T>(T defaultValue, int length)
    {
        T[] arr = new T[length];
        for (int i = 0; i < length; i++)
        {
            arr[i] = defaultValue;
        }
        return arr;
    }
    private void _ArrayOutput<T>(T[] inserted, T[] resulted)
    {
        if (inserted.Length != resulted.Length) 
        {
            _output.WriteLine($"Lengths are different >> Inserted: {inserted.Length}, resulted: {resulted.Length}");
            return;
        }
        int length = inserted.Length;
        string seperator = ", ";
        StringBuilder insertedBuilder = new(length);
        StringBuilder resultedBuilder = new(length);
        for (int i = 0; i < length; i++)
        {
            insertedBuilder.Append(Convert.ToString(inserted[i]));
            resultedBuilder.Append(Convert.ToString(resulted[i]));
            if (i >= length - 1) continue;
            insertedBuilder.Append(seperator);
            resultedBuilder.Append(seperator);
        }
        _output.WriteLine($"Expected >> {insertedBuilder.ToString()}{_outputSeperator}Resulted >> {resultedBuilder.ToString()}");
    }
    private void _PropertyOutput(Matrix2D<int> matrix, int expectedLength, int expectedWidth, int expectedCount)
    {
        StringBuilder builder = new();
        builder.Append($"Matrix >> Length: {Convert.ToString(matrix.Length)}, Width: {Convert.ToString(matrix.Width)}, Count: {Convert.ToString(matrix.Count)}");
        builder.Append(_outputSeperator);
        builder.Append($"Expected >> Lenght: {Convert.ToString(expectedLength)}, Width: {Convert.ToString(expectedWidth)}, Count: {Convert.ToString(expectedCount)}");
        _output.WriteLine(builder.ToString());
    }
    private void _IndexOutput(int inserted, int length, int width, int x, int y)
    {
        StringBuilder builder = new();
        builder.Append($"Params >> Inserted: {Convert.ToString(inserted)}, Length: {Convert.ToString(length)}, Width: {Convert.ToString(width)}");
        builder.Append(_outputSeperator);
        builder.Append($"Internal >> X: {Convert.ToString(x)}, Y: {Convert.ToString(y)}");
        _output.WriteLine(builder.ToString());
    }
}