using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using EvilGiraffes.Functools;

namespace EvilGiraffes.Tests;
public class CompletionRunnerTest
{
    private ITestOutputHelper _output;
    private readonly string[] _argumentTitleArray;
    private readonly string[] _triesTitleArray;
    private static readonly Random _random = new();
    private const int _maxTries = 10;
    private const int _amountAdditionValues = 3;
    private const int _maxRandomizerValue = 1000;
    public CompletionRunnerTest(ITestOutputHelper output)
    {
        _output = output;
        _argumentTitleArray = new[] { "Initial", "Expected", "Actual" };
        _triesTitleArray = new[] { "Initial", "Current Tries" };
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void VoidOneArgTest(int initialValue, int[] additionValues)
    {
        _RunVoidTest(
            initialValue,
            new[] { additionValues[0] }
        );
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void VoidTwoArgTest(int initialValue, int[] additionValues)
    {
        _RunVoidTest(
            initialValue,
            new[] { additionValues[0], additionValues[1] }
        );
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void VoidThreeArgTest(int initialValue, int[] additionValues)
    {
        _RunVoidTest(
            initialValue,
            new[] { additionValues[0], additionValues[1], additionValues[2] }
        );
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void ReturnOneArgTest(int initialValue, int[] additionValues)
    {
        _RunReturnTest(
            initialValue,
            new[] { additionValues[0] }
        );
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void ReturnTwoArgTest(int initialValue, int[] additionValues)
    {
        _RunReturnTest(
            initialValue,
            new[] { additionValues[0], additionValues[1] }
        );
    }
    [Theory]
    [MemberData(nameof(IntValues))]
    public void ReturnThreeArgTest(int initialValue, int[] additionValues)
    {
        _RunReturnTest(
            initialValue,
            new[] { additionValues[0], additionValues[1], additionValues[2] }
        );
    }
    [Theory]
    [MemberData(nameof(Runs))]
    public void VoidTriesTest(int runs)
    {
        RunUntillComplete runner = _GetTriesRunner(runs);
        runner.Execute();
        _output.WriteLine(_GetOutputFormat(_triesTitleArray), runs, runner.CurrentTries);
        Assert.Equal(runs, runner.CurrentTries);
    }
    [Theory]
    [MemberData(nameof(Runs))]
    public void ReturnTriesTest(int runs)
    {
        RunUntillComplete<bool> runner = _GetTriesRunner<bool>(runs);
        runner.Execute();
        _output.WriteLine(_GetOutputFormat(_triesTitleArray), runs, runner.CurrentTries);
        Assert.Equal(runs, runner.CurrentTries);
    }
    public static IEnumerable<Object[]> IntValues()
    {
        for (int i = 0; i < RunConfig.TotalRuns; i++)
        {
            int initialValue = _random.Next(_maxRandomizerValue);
            int[] additionValues = new int[_amountAdditionValues];
            for (int j = 0; j < _amountAdditionValues; j++)
            {
                additionValues[j] = _random.Next(_maxRandomizerValue);
            }
            yield return new object[] {initialValue, additionValues};
        }
    }
    public static IEnumerable<Object[]> Runs()
    {
        int totalRuns = RunConfig.TotalRuns;
        if (totalRuns > _maxTries) totalRuns = _maxTries;
        List<int> cache = new(totalRuns);
        for (int i = 0; i < totalRuns; i++)
        {
            yield return new object[] { _TriesGetRandomNum(ref cache) };
        }
    }
    private static int _TriesGetRandomNum(ref List<int> cache)
    {
        int result;
        do
        {
            result = _random.Next(1, _maxTries + 1);
            if (cache.Count >= _maxTries) break;
        } while(cache.Contains(result));
        cache.Add(result);
        return result;
    }
    private void _RunReturnTest(int initialValue, int[] additionValues)
    {
        int additionValue = _AddValues(additionValues);
        int expectedValue = _ReturnGetExpectedValue(initialValue, additionValue);
        int actualValue = _ReturnGetActualValue<int>(initialValue, additionValue);
        _output.WriteLine(_GetOutputFormat(_argumentTitleArray), initialValue, expectedValue, actualValue);
        Assert.Equal(expectedValue, actualValue);
    }
    private void _RunVoidTest(int initialValue, int[] additionValues)
    {
        int additionValue = _AddValues(additionValues) * _maxTries;
        int expectedValue = _VoidGetExpectedValue(initialValue, additionValue);
        int actualValue = _VoidGetActualValue(initialValue, additionValue);
        _output.WriteLine(_GetOutputFormat(_argumentTitleArray), initialValue, expectedValue, actualValue);
        Assert.Equal(expectedValue, actualValue);
    }
    private int _ReturnGetExpectedValue(int initialValue, int additionValue) => initialValue + additionValue;
    private int _ReturnGetActualValue<T>(int initialValue, int additionValue)
    {
        RunUntillComplete<int> runner = new(
                (RunUntillComplete<int> runner) => initialValue + additionValue,
            _maxTries
        );
        return runner.Execute();
    }
    private int _VoidGetExpectedValue(int initialValue, int additionValue) => initialValue + additionValue * _maxTries;
    private int _VoidGetActualValue(int initialValue, int additionValue)
    {
        PropertyContainer container = initialValue;
        RunUntillComplete runner = new(
                (RunUntillComplete runner) => container.Value += additionValue,
            _maxTries
        );
        runner.Execute();
        return container;
    }
    private RunUntillComplete _GetTriesRunner(int runs)
    {
        Action<RunUntillComplete> func = (runner) => {
            if (runner.CurrentTries == runs) runner.Break();
        };
        return new(func, _maxTries);
    }
    private RunUntillComplete<T?> _GetTriesRunner<T>(int runs)
    {
        Func<RunUntillComplete<T?>, T?> func = (runner) => {
            if (runner.CurrentTries == runs) runner.Break();
            // Discarded return value.
            return default;
        };
        return new(func, _maxTries);
    }
    private int _AddValues(int[] values)
    {
        int result = 0;
        for (int i = 0; i < values.Length; i++)
        {
            result += values[i];
        }
        return result;
    }
    private string _GetOutputFormat(string[] titleArray)
    {
        StringBuilder builder = new();
        for (int i = 0; i < titleArray.Length; i++)
        {
            if (i != 0) _AddDelimiter(ref builder);
            _AddFormat(ref builder, titleArray[i], i);
        }
        return builder.ToString();
    }
    private void _AddFormat(ref StringBuilder builder, string title, int formatIndex)
    {
        builder.Append(title)
        .Append(Output.TitleDelimiter)
        .Append('{')
        .Append(formatIndex)
        .Append('}');
    }
    private void _AddDelimiter(ref StringBuilder builder) => builder.Append(Output.Delimiter);
    private class PropertyContainer
    {
        public int Value { get; set; }
        public PropertyContainer()
        {
            Value = 0;
        }
        public static implicit operator int(PropertyContainer container) => container.Value;
        public static implicit operator PropertyContainer(int value) => new PropertyContainer() { Value = value };
    }
}