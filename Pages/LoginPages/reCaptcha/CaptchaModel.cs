using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace Northwind.Interface.Server.Pages.LoginPages.reCaptcha
{
    public class CaptchaModel<TLocal>: BaseComponentModel<TLocal>, IAsyncDisposable
    {
        protected IJSObjectReference module { get; set; }

        protected string UniqueId = Guid.NewGuid().ToString();
        [Inject] private IJSRuntime jsruntime { get; set; }

        [Inject] private CaptchaApi captchaApi { get; set; }

        public bool EnableCreateUserCaptcha { get; set; }

        [Parameter]
        public EventCallback<bool> CaptchaSuccessful { get; set; }

        private int widgetId;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    module = await jsruntime.InvokeAsync<IJSObjectReference>("import", "./Pages/LoginPages/reCaptcha/Captcha.razor.js");
                    await module.InvokeVoidAsync("init");
                    Thread.Sleep(500);
                    widgetId = await module.InvokeAsync<int>("googleRecaptcha", DotNetObjectReference.Create(this), UniqueId, "6LcFdOIkAAAAAHTLyZCy3LyoX6nwnosWtHeg8fxS");
                }
            }
            catch (Exception )
            {
               // await component.ShowErrorMessage(ex.Message);
            }
        }

        [JSInvokable, EditorBrowsable(EditorBrowsableState.Never)]
        public async void CallbackOnSuccess(string response)
        {
            var result = await captchaApi.Post(response);
            if (result.Success)
            {
                if (CaptchaSuccessful.HasDelegate)
                   await CaptchaSuccessful.InvokeAsync(true);
            }
        }

        [JSInvokable, EditorBrowsable(EditorBrowsableState.Never)]
        public void CallbackOnExpired(string response)
        {
            var responss = response;
        }

        public async Task<string> GetCaptchaRespons()
        {
            try
            {
                return await module.InvokeAsync<string>("getResponse", widgetId);
            }
            catch(Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }
            return string.Empty;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (module != null)
                    await module.DisposeAsync();
            }
            catch(JSDisconnectedException )
            {

            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }
        }
    }
}
