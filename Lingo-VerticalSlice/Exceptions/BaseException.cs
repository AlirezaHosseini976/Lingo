namespace Lingo_VerticalSlice.Exceptions;

public class BaseException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }

    public BaseException(string message, int statusCode,string code) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }
    public BaseException(string message) : this(message, 500, "InternalServerError")
    {
    }
    
}