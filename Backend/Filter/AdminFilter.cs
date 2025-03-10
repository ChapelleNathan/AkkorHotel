using System.Security.Claims;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Filter;

public class AdminFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        if (role is null || RoleEnum.User.ToString().Equals(role))
        {
            var response = new HttpResponse<object>()
            {
                Response = null,
                HttpCode = 401,
                ErrorMessage = ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401Authorization)
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        
        base.OnActionExecuting(context);
    }
}