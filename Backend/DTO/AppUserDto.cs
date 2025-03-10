using System.Security.Claims;

namespace Backend.DTO;

public class AppUserDto : ClaimsPrincipal
{
    public AppUserDto(IHttpContextAccessor httpContextAccessor) : base (httpContextAccessor.HttpContext!.User) {}
    
    public string Email => FindFirst(ClaimTypes.Email).Value;
    public string Id => FindFirst(ClaimTypes.NameIdentifier).Value;
    public string Role => FindFirst(ClaimTypes.Role).Value;
}