namespace Backend.HttpResponse;

public class HttpResponseException(int statusCode, string message, object? value = null): Exception
{
    public int StatusCode { get; } = statusCode;
    public object? Value { get; } = value;
    public new string Message { get; } = message;
}