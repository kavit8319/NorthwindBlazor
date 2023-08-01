using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using System.Security.Claims;
using System.Security.Principal;

namespace Northwind.Interface.Server.Authentification
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        BaseHttpClient httpClient { get; set; }
        public CustomAuthenticationStateProvider(BaseHttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = string.Empty;
            ClaimsIdentity identiy;
            try
            {
                token = await httpClient.LocalStoreService.GetItemAsync(Constans.AccessToken);
               
                if (!string.IsNullOrEmpty(token))
                {
                    var user = await (await httpClient.Client()).GetUserByAccessTokenAsync(token);
                    identiy = GetClaimsIdentity(user.Data);
                }
                else
                    identiy = new ClaimsIdentity();
                var userIdentity = new ClaimsPrincipal(identiy);


                return await Task.FromResult(new AuthenticationState(userIdentity));
            }

            catch (ApiException e)
            {

                if (e.Message.Contains("TheTokenIsExpired"))
                {
                    await httpClient.LocalStoreService.RemoveItemAsync(Constans.AccessToken);
                    await httpClient.LocalStoreService.RemoveItemAsync(Constans.RefreshToken);
                    identiy = new ClaimsIdentity();
                    var userIdentity = new ClaimsPrincipal(identiy);
                    return await Task.FromResult(new AuthenticationState(userIdentity));
                }
                else
                    throw e;
            }
        }
        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            var state = await this.GetAuthenticationStateAsync();
            var authenticationStateProviderUser = state.User;
            return authenticationStateProviderUser;
        }
        public async void MarkUserAsAuthenticated(UserReturn userView)
        {
            await httpClient.LocalStoreService.SetItemAsync(Constans.AccessToken, userView.AccessToken);
            await httpClient.LocalStoreService.SetItemAsync(Constans.RefreshToken, userView.RefreshToken);
            var identiy = GetClaimsIdentity(new UserReturn() { EmailAddress = userView.EmailAddress, RoleId = userView.RoleId, RoleName = userView.RoleName });
            // var identiy = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(identiy);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await httpClient.LocalStoreService.RemoveItemAsync(Constans.RefreshToken);
            await httpClient.LocalStoreService.RemoveItemAsync(Constans.AccessToken);
            //var identiy = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, emailAddress) }, "apiauth_type");
            var identiy = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identiy);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        public async Task StateChangedAsync()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            NotifyAuthenticationStateChanged(authState);

        }
        private ClaimsIdentity GetClaimsIdentity(UserReturn user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.EmailAddress != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.Role, user.RoleName),
                   // new Claim(ClaimTypes.Role, user.Role.RoleDesc),
                   // new Claim("IsUserEmployedBefore1990", IsUserEmployedBefore1990(user))
                }, "apiauth_type");
            }

            return claimsIdentity;
        }

        //private string IsUserEmployedBefore1990(User user)
        //{
        //    if (user.HireDate.Value.Year < 1990)
        //        return "true";
        //    return "false";
        //}
    }
}
