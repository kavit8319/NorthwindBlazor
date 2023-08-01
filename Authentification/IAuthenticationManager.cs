using Northwind.Interface.Server.ClientWebApi;
using System.Security.Claims;

namespace Northwind.Interface.Server.Authentification
{
    public interface IAuthenticationManager 
    {
        Task<Interface.Shared.IResult> Login(UserReturn user,bool rememberMe);

        Task<Interface.Shared.IResult> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
    }
}
