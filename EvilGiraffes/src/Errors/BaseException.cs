using System.Runtime.Serialization;

namespace EvilGiraffes.Errors;
/// <summary>
/// Base class for exceptions in this project. 
/// </summary>
public abstract class BaseException : System.Exception
{
    public BaseException(): base() {}
    public BaseException(string message) : base(message) {}
    public BaseException(string message, System.Exception inner) : base(message, inner) {}
    public BaseException(SerializationInfo info, StreamingContext context) : base(info, context) {}
}