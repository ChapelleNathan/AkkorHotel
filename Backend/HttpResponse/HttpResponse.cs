namespace Backend.HttpResponse;

public class HttpResponse<T>
{
    public T? Response { get; set; }
    public int HttpCode { get; set; } = 200;
    public string? ErrorMessage { get; set; }
}