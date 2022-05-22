namespace EvilGiraffes.Functools;
public abstract class RunUntillCompleteBase
{
    protected bool _break;
    protected int _maxTries;
    protected int _tries;
    protected object _padlock = new();
    /// <summary>
    /// Base implementation of the completion runner constructor.
    /// </summary>
    /// <param name="maxTries">Maximum amount of tries untill it breaks out of loop, defaults to -1 which means it will run infinitely.</param>
    protected RunUntillCompleteBase(int maxTries = -1)
    {
        _maxTries = maxTries;
        _break = false;
        _tries = 0;
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
        if (_break is true) return false;
        if (_maxTries == -1) return true;
        if (_tries >= _maxTries) return false;
        _tries++;
        return true;
    }
}