using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Northwind.Interface.Server.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            ,AllowRefresh=false,
            IsPersistent=true
        };

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult> SignInPost(SigninData value)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, value.Email),
            new Claim(ClaimTypes.Name,  value.Email),
            new Claim(ClaimTypes.Role,  value.RoleName)
        };

            var claimsIdentity = new ClaimsIdentity(claims,
                                                    CookieAuthenticationDefaults.AuthenticationScheme);
            COOKIE_EXPIRES.IsPersistent = value.IsPersistent;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity),
                                          COOKIE_EXPIRES);

            return Redirect("/");
        }

        [HttpPost]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }
    }

    public class SigninData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
        public string RoleName { get; set;}
    }
}
