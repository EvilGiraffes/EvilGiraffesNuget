namespace EvilGiraffes.Functools;
/// <summary>
/// A basic completion runner which will return void.
/// </summary>
public class RunUntillComplete: RunUntillCompleteBase
{
    private IRunner _runner;
    /// <summary>
    /// Will construct a new instance of a completion runner.
    /// </summary>
    /// <param name="runner">The runner function in which should run in the loop.</param>
    /// <param name="maxTries"><inheritdoc cref="RunUntillCompleteBase.RunUntillCompleteBase" /></param>
    public RunUntillComplete(IRunner runner, int maxTries = -1): base(maxTries)
    {
        _runner = runner;
    }
    /// <summary>
    /// Starts the loop.
    /// </summary>
    public void Execute()
    {
        lock (_padlock)
        {
            _Run();
        }
    }
    private void _Run()
    {
        bool success;
        do
        {
            success = _runner.Execute(this);
        } while (success is false && _Continue() is true);
    }
}
/// <summary>
/// A basic completion runner which will return set value.
/// </summary>
/// <typeparam name="TReturn">The value to return.</typeparam>
public class RunUntillComplete<TReturn>: RunUntillCompleteBase
{
    private IRunner<TReturn> _runner;
    /// <inheritdoc cref="RunUntillComplete.RunUntillComplete"/>
    public RunUntillComplete(IRunner<TReturn> runner, int maxTries = -1): base(maxTries)
    {
        _runner = runner;
    }
    /// <summary>
    /// <inheritdoc cref="RunUntillComplete.Execute"/>
    /// </summary>
    /// <returns>Returns the return value of the runner function.</returns>
    public TReturn Execute()
    {
        lock (_padlock)
        {
            return _Run();
        }
    }
    private TReturn _Run()
    {
        TReturn result;
        bool success;
        do
        {
            (TReturn, bool) executionResult = _runner.Execute(this);
            result = executionResult.Item1;
            success = executionResult.Item2;
        } while (success is false && _Continue() is true);
        return result; 
    }
}
