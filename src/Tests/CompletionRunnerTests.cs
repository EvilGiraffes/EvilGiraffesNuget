using Xunit;
using Xunit.Sdk;
using Xunit.Abstractions;

using EvilGiraffes.Functools;

namespace EvilGiraffes.Tests;
public class CompletionRunnerTests
{
    private ITestOutputHelper _output;
    private Random _random;
    private readonly string _outputSeperator;
    public CompletionRunnerTests(ITestOutputHelper output)
    {
        _output = output;
        _random = new();
        _outputSeperator = "\t\t";
    }
    [Fact]
    public void RunUntillCompleteReturnValueTest()
    {
        int endValue = _random.Next(1, 30);
        RunUntillComplete<int> runner = new(
            new RunnerReturnValue(_Increment, endValue)
            );
        int expected = endValue - 1;
        int actual = runner.Execute();
        _output.WriteLine($"Expected: {expected}{_outputSeperator}Actual: {actual}");
        Assert.Equal(expected, actual);
    }
    private int _Increment(int value) => value + 1;
    private class RunnerReturnValue: IRunner<int>
    {
        private Func<int, int> _func;
        private int _endValue;
        private int _currentValue;
        public RunnerReturnValue(Func<int, int> func, int endValue)
        {
            _func = func;
            _endValue = endValue;
            _currentValue = 0;
        }
        public (int, bool) Execute(RunUntillComplete<int> runner)
        {
            int result = _func(_currentValue);
            if (result >= _endValue) return (_currentValue, true);
            _currentValue = result;
            return (_currentValue, false);
        }
    }
}