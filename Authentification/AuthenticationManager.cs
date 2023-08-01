using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Http;
using Microsoft.JSInterop;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Controllers;
using Northwind.Interface.Server.Shared;
using Northwind.Interface.Shared;
using System.Security.Claims;

namespace Northwind.Interface.Server.Authentification
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private BaseHttpClient httpClient;
      
        private IJSRuntime JSRunTime { get; set; }
        private ILocalStorageService localStorage;
        private NavigationManager navigationManager { get; set; }
        public AuthenticationManager(BaseHttpClient _httpClient, AuthenticationStateProvider aut,NavigationManager _navigation,IJSRuntime _jsRuntime,ILocalStorageService _localStorage)
        {
            httpClient = _httpClient;
            _authenticationStateProvider = aut;
            navigationManager = _navigation;
            JSRunTime = _jsRuntime;
            localStorage = _localStorage;
        }
        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<Interface.Shared.IResult> Login(UserReturn user, bool rememberMe)
        {
            var res = await (await httpClient.Client()).LoginAsync(user.EmailAddress, user.Password);
            if (res.Succeeded)
            {
                await httpClient.LocalStoreService.SetItemAsync(Constans.AccessToken, res.Data.AccessToken);
                await httpClient.LocalStoreService.SetItemAsync(Constans.RefreshToken, res.Data.RefreshToken);
                await ((CustomAuthenticationStateProvider)this._authenticationStateProvider).StateChangedAsync();

                //var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
                //await authModule.InvokeVoidAsync("SignIn", user.EmailAddress,user.Password,rememberMe,res.Data.RoleName, "/");
                //var buildService = httpClient.ServiceCollectionProv.ServiceCollection.BuildServiceProvider();
                //var httpClientFactory = buildService.GetRequiredService<IHttpClientFactory>();
                //var httpClientLocal = httpClientFactory.CreateClient("UserServiceSimpleLocalAuth");

                //await httpClientLocal.PostAsJsonAsync<SigninData>("/api/auth/signin", new SigninData { Email = user.EmailAddress, Password = user.Password, RoleName = res.Data.RoleName, IsPersistent = rememberMe });

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync(res.Messages);
            }
        }

        public async Task<Interface.Shared.IResult> Logout()
        {
            await httpClient.LocalStoreService.RemoveItemAsync(Constans.AccessToken);
            await httpClient.LocalStoreService.RemoveItemAsync(Constans.RefreshToken);
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            //var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            //await authModule.InvokeVoidAsync("SignOut", "/login");
            navigationManager.NavigateTo("/login");
            return await Result.SuccessAsync();
        }

        public async Task<string> RefreshToken()
        {
            var resAccessToken = await httpClient.LocalStoreService.GetItemAsync(Constans.AccessToken);
            var resRefreshToken = await httpClient.LocalStoreService.GetItemAsync(Constans.RefreshToken);
            var res = await (await httpClient.Client()).RefreshTokenAsync(new RefreshRequest { AccessToken = resAccessToken, RefreshToken = resRefreshToken });
            if (!res.Succeeded)
                throw new ApiException(string.Join(",", res.Messages), 500, "", null, null);
            await httpClient.LocalStoreService.SetItemAsync(Constans.AccessToken, res.Data.AccessToken);
            await httpClient.LocalStoreService.SetItemAsync(Constans.RefreshToken, res.Data.RefreshToken);
            return res.Data.AccessToken;
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }

        public async Task<string> TryRefreshToken()
        {
            //check if token exists
            var availableToken = await httpClient.LocalStoreService.GetItemAsync(Constans.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }
    }
}
