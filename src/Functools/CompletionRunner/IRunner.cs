namespace EvilGiraffes.Functools;
/// <summary>
/// Runner which will be ran in a completion runner with a void return.
/// </summary>
public interface IRunner
{
    /// <summary>
    /// Executes the loop.
    /// </summary>
    /// <param name="runner">Will give you the instance of the runner which is looping the function.</param>
    /// <returns>TRUE if function was successful, FALSE if function was unsuccessful</returns>
    public bool Execute(RunUntillComplete runner);
}
/// <summary>
/// Runner which will be ran in a completion runner with a return type.
/// </summary>
/// <typeparam name="TReturn">Return type from the function.</typeparam>
public interface IRunner<TReturn>
{
    /// <summary>
    /// Executes the loop.
    /// </summary>
    /// <param name="runner"><inheritdoc cref="IRunner.Execute" path="/param[@name='runner']"/></param>
    /// <returns>TReturn is the return value you want to return in the loop. Bool returns TRUE if function was successful, FALSE if function was unsuccessful</returns>
    public (TReturn, bool) Execute(RunUntillComplete<TReturn> runner);
}