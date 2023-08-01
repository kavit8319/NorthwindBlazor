using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Northwind.Interface.Server.Controllers
{
    public class CookieAuthenticationEvents : Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        //private readonly Factory _factory;

        //public CookieAuthenticationEvents(Factory factory)
        //{
        //    _factory = factory;
        //}

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // Validate the Principal
            var userPrincipal = context.Principal;

            // If not valid, Sign out
            if (userPrincipal.Identity.Name != "kazakov1551983@gmail.com")
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
