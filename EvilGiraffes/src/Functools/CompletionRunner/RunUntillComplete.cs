namespace EvilGiraffes.Functools;
/// <summary>
/// A basic completion runner which will return void.
/// </summary>
public class RunUntillComplete: RunUntillCompleteBase
{
    private Action<RunUntillComplete> _func;
    /// <summary>
    /// Will construct a new instance of a completion runner. 
    /// </summary>
    /// <param name="func">The function that should run in the loop.</param>
    /// <param name="maxTries"><inheritdoc cref="RunUntillCompleteBase.RunUntillCompleteBase" /> Number is Inclusive.</param>
    public RunUntillComplete(Action<RunUntillComplete> func, int maxTries = -1): base(maxTries) 
    {
        _func = func;
    } 
    /// <summary>
    /// Will construct a new instance of a completion runner.
    /// </summary>
    /// <param name="runner">The runner function in which should run in the loop.</param>
    /// <param name="maxTries"><inheritdoc cref="RunUntillCompleteBase.RunUntillCompleteBase" /> Number is Inclusive.</param>
    public RunUntillComplete(IRunner runner, int maxTries = -1): this(runner.Execute, maxTries) {}
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
        do
        {
            CurrentTries++;
            _func(this);
        } while (_Continue() is true);
    }
}
/// <summary>
/// A basic completion runner which will return set value.
/// </summary>
/// <typeparam name="TReturn">The value to return.</typeparam>
public class RunUntillComplete<TReturn>: RunUntillCompleteBase
{
    private Func<RunUntillComplete<TReturn>, TReturn> _func;
    /// <summary>
    /// Will construct a new instance of a completion runner.
    /// </summary>
    /// <param name="func">The function that should run in the loop.</param>
    /// <param name="maxTries"><inheritdoc cref="RunUntillCompleteBase.RunUntillCompleteBase" /> Number is Inclusive.</param>
    public RunUntillComplete(Func<RunUntillComplete<TReturn>, TReturn> func, int maxTries = -1): base(maxTries) 
    {
        _func = func;
    }
    /// <summary>
    /// Will construct a new instance of a completion runner.
    /// </summary>
    /// <param name="runner">The runner function in which should run in the loop.</param>
    /// <param name="maxTries"><inheritdoc cref="RunUntillCompleteBase.RunUntillCompleteBase" /> Number is Inclusive.</param>
    public RunUntillComplete(IRunner<TReturn> runner, int maxTries = -1): this(runner.Execute, maxTries) {}
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
        do
        {
            CurrentTries++;
            result = _func(this);
        } while (_Continue() is true);
        return result; 
    }
}
