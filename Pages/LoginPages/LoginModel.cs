using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Authentification;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Components;
using Northwind.Interface.Server.Shared;
using Northwind.Interface.Shared;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.SplitButtons;
using System;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;

namespace Northwind.Interface.Server.Pages.LoginPages
{
    public class LoginModel<TLocal> : BaseComponentModel<TLocal>
    {
        [Inject]
        protected IAuthenticationManager authenticated { get; set; }

        [Inject]
        protected CustomAuthenticationStateProvider customAuthenticationStateProvider { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        public CustomToastService   ToastService { get; set; }
        protected SfProgressButton sfButon { get; set; }
        public UserLogoneModel UserLogone { get; set; } = new();

        [CascadingParameter] public CenteredCardLayout centerCard { get; set; }

        [Parameter]
        public string Code { get; set; }

      
        protected reCaptcha.Captcha captcha { get; set; } = new();
        protected bool EnableLoginCaptcha { get; set; } = true;
        protected override void OnInitialized()
        {
            if (centerCard != null)
                centerCard.FormName = @localResource["Sign_in"];
        }
       
        protected override async Task OnParametersSetAsync()
        {
            StringValues code;
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (!string.IsNullOrEmpty(uri.Query))
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("code", out code))
                {
                    var result = ParseQueryString(uri.Query);
                    var res = await (await httpClient.Client()).ActivateUserByCodeAsync(result["code"]);
                    if (res.Succeeded)
                        ToastService.ShowToast(localResource["userActivated"],MessageSeverity.Success);
                    else
                        ToastService.ShowToast(localResource["errorUserActivated"],MessageSeverity.Error);
                }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("userCreated", out var userCreated))
                {
                    if (userCreated.Any())
                    {
                        var result = bool.Parse(userCreated.ToString());
                        if (result)
                            ToastService.ShowToast(localResource["userCreatedNeedActivation"],MessageSeverity.Success);
                    }
                }
            }
        }

        public Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }
        protected override async Task OnInitializedAsync()
        {
          
            var state = await customAuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())) && state.User.Identity.IsAuthenticated)
            {
                navigationManager.NavigateTo("/", true);
            }
        }

        public void SuccsessfullCaptcha(bool resultCaptcha)
        {
            EnableLoginCaptcha = resultCaptcha;
        }
        protected async Task SubmitAsync()
        {
           
            var result = await authenticated.Login(new UserReturn { EmailAddress = UserLogone.EmailAddress, Password = UserLogone.Password }, UserLogone.RememberMe);
            await sfButon.EndProgressAsync();
            if (!result.Succeeded)
                ToastService.ShowToast(localResource[result.Messages.First()],MessageSeverity.Error);

        }
    }
}
