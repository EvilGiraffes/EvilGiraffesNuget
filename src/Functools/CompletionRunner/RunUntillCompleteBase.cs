namespace EvilGiraffes.Functools;
public abstract class RunUntillCompleteBase
{
    public int CurrentTries { get; protected set; }
    protected readonly int _maxTries;
    protected readonly object _padlock;
    protected bool _break;
    /// <summary>
    /// Base implementation of the completion runner constructor.
    /// </summary>
    /// <param name="maxTries">Maximum amount of tries untill it breaks out of loop, defaults to -1 which means it will run infinitely.</param>
    protected RunUntillCompleteBase(int maxTries = -1)
    {
        CurrentTries = 0;
        _maxTries = maxTries;
        _padlock = new();
        _break = false;
    }
    /// <summary>
    /// Will break the loop.
    /// </summary>
    public virtual void Break() => _break = true;
    /// <summary>
    /// Checks whether or not the loop should continue.
    /// </summary>
    /// <returns>TRUE if the loop should continue, FALSE if the loop should break.</returns>
    protected virtual bool _Continue()
    {
        if (_ShouldBreak()) return false;
        if (_HasNoLimit()) return true;
        if (_OverTheBounds()) return false;
        return true;
    }
    protected bool _ShouldBreak() => _break is true;
    protected bool _HasNoLimit() => _maxTries == -1;
    protected bool _OverTheBounds() => CurrentTries >= _maxTries;
}