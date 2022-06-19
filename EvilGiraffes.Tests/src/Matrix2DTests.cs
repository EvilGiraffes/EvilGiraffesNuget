using System;
using System.Collections.Generic;
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
    private static readonly Random _random = new();
    public Matrix2DTest(ITestOutputHelper output)
    {
        _output = output;
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
        Assert.Equal(insertArray, matrix.Row(y));
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void RowOffsetTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        OffsetData data = new(
            indexMax: length,
            offsetMax: width + 1,
            arrayFull: width
        );
        int[] insertArray = _PopulateArray<int>(inserted, data.ArrayLength);
        matrix.Row(data.Index, insertArray, data.Offset);
        int[] rowSlice = _GetArraySlice<int>(
            matrix.Row(data.Index), data.Offset
        );
        Assert.Equal(rowSlice, insertArray);
    }
    [Theory]
    [MemberData(nameof(LengthWidthData))]
    public void RowSetterExceptionTest(int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int y = _random.Next(length);
        _RunOutOfBoundsTest(y, width, matrix.Row);
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void ColumnTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int x = _random.Next(width);
        int[] insertArray = _PopulateArray<int>(inserted, length);
        matrix.Column(x, insertArray);
        Assert.Equal(insertArray, matrix.Column(x));
    }
    [Theory]
    [MemberData(nameof(InsertLengthWidthData))]
    public void ColumnOffsetTest(int inserted, int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        OffsetData data = new(
            indexMax: width,
            offsetMax: length + 1,
            arrayFull: length
        );
        int[] insertArray = _PopulateArray<int>(inserted, data.ArrayLength);
        matrix.Column(data.Index, insertArray, data.Offset);
        int[] columnSlice = _GetArraySlice<int>(
            matrix.Column(data.Index), data.Offset
        );
        Assert.Equal(insertArray, columnSlice);
    }
    [Theory]
    [MemberData(nameof(LengthWidthData))]
    public void ColumnSetterExceptionTest(int length, int width)
    {
        Matrix2D<int> matrix = new(width, length, 0);
        int x = _random.Next(width);
        _RunOutOfBoundsTest(x, length, matrix.Column);
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
        _output.WriteLine($"Expected{Output.TitleDelimiter}{nameof(MatrixInitializedException)}{Output.Delimiter}Outputted{Output.TitleDelimiter}{exception.GetType().Name}");
        Assert.IsType<MatrixInitializedException>(exception);
    }
    [Theory]
    [MemberData(nameof(LengthWidthData))]
    public void InitializationFunctionTest(int length, int width)
    {
        Matrix2D<InitStruct> matrix = new(width, length);
        Cache<InitStruct> cache = new(20);
        int currentIndex = 0;
        Func<InitStruct> func = () => {
            InitStruct obj = new(currentIndex);
            currentIndex++;
            return obj;
        };
        matrix.Initalize(func);
        foreach (InitStruct obj in matrix)
        {
            Assert.False(
                cache.Contains(obj)
            );
            cache.Add(obj);
        }
    }
    public static IEnumerable<object[]> LengthWidthData()
    {
        for (int i = 0; i < RunConfig.TotalRuns; i++)
        {
            yield return new object[] { _random.Next(RunConfig.RandomMinNum, RunConfig.RandomMaxNum), _random.Next(RunConfig.RandomMinNum, RunConfig.RandomMaxNum) };
        }
    }
    public static IEnumerable<object[]> InsertLengthWidthData()
    {
        for (int i = 0; i < RunConfig.TotalRuns; i++)
        {
            yield return new object[] { _random.Next(RunConfig.RandomMinNum, RunConfig.RandomMaxNum), _random.Next(RunConfig.RandomMinNum, RunConfig.RandomMaxNum), _random.Next(RunConfig.RandomMinNum, RunConfig.RandomMaxNum) };
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
    private T[] _GetArraySlice<T>(T[] array, int offset)
    {
        int adjustedArrayLength = array.Length - offset;
        T[] result = new T[array.Length - offset];
        for (int i = 0; i < adjustedArrayLength; i++)
        {
            result[i] = array[i + offset];
        }
        return result;
    }
    private void _PropertyOutput(Matrix2D<int> matrix, int expectedLength, int expectedWidth, int expectedCount)
    {
        StringBuilder builder = new();
        builder.Append($"Matrix{Output.TitleDelimiter}Length: {Convert.ToString(matrix.Length)}, Width: {Convert.ToString(matrix.Width)}, Count: {Convert.ToString(matrix.Count)}");
        builder.Append(Output.Delimiter);
        builder.Append($"Expected{Output.TitleDelimiter}Lenght: {Convert.ToString(expectedLength)}, Width: {Convert.ToString(expectedWidth)}, Count: {Convert.ToString(expectedCount)}");
        _output.WriteLine(builder.ToString());
    }
    private void _IndexOutput(int inserted, int length, int width, int x, int y)
    {
        StringBuilder builder = new();
        builder.Append($"Params{Output.TitleDelimiter}Inserted: {Convert.ToString(inserted)}, Length: {Convert.ToString(length)}, Width: {Convert.ToString(width)}");
        builder.Append(Output.Delimiter);
        builder.Append($"Internal{Output.TitleDelimiter}X: {Convert.ToString(x)}, Y: {Convert.ToString(y)}");
        _output.WriteLine(builder.ToString());
    }
    private void _RunOutOfBoundsTest(int index, int size, Action<int, int[], int> exceptionFunc)
    {
        int[] insertArray = {1, 2, 3};
        int offset = size - 2;
        var exception = Record.Exception(
            () => exceptionFunc(index, insertArray, offset)
        );
        _output.WriteLine($"Expected{Output.TitleDelimiter}{nameof(MatrixOutOfBoundsException)}{Output.Delimiter}Outputted{Output.TitleDelimiter}{exception.GetType().Name}");
        Assert.IsType<MatrixOutOfBoundsException>(exception);
    }
    private readonly struct OffsetData
    {
        public int Index { get; init; }
        public int Offset { get; init; }
        public int ArrayLength { get; init; }
        public OffsetData(int indexMax, int offsetMax, int arrayFull)
        {
            Index = _random.Next(indexMax);
            Offset = _random.Next(offsetMax);
            ArrayLength = arrayFull - Offset;
        }
    }
    private readonly struct InitStruct
    {
        public int Value { get; init; }
        public InitStruct(int value)
        {
            Value = value;
        }
        public static implicit operator int(InitStruct container) => container.Value;
        public static implicit operator InitStruct(int value) => new InitStruct(value);
    }
    private class Cache<T>
    {
        public int MaxSize { get; set; }
        private List<T> _cache;
        public Cache(int maxSize)
        {
            _cache = new(maxSize);
            MaxSize = maxSize;
        }
        public void Add(T value)
        {
            if (MaxSize <= _cache.Count && _cache.Count > 0) _cache.RemoveAt(0);
            _cache.Add(value);
        }
        public bool Contains(T value) => _cache.Contains(value);
    }
}