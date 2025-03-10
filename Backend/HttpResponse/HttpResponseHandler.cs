using Microsoft.AspNetCore.Mvc;

namespace Backend.HttpResponse;

public class HttpResponseHandler : ControllerBase
{
    public ActionResult<HttpResponse<T>> Handle<T>(HttpResponse<T> response)
    {
        ActionResult<HttpResponse<T>> httpResponse = Ok(response);
        switch (response.HttpCode)   
        {
            case 400:
                httpResponse = BadRequest(response);
                break;
            case 404:
                httpResponse = NotFound(response);
                break;
            case 401:
                httpResponse = Unauthorized(response);
                break;
            case 500:
                httpResponse = StatusCode(500, response);
                break;
        }

        return httpResponse;
    }
}