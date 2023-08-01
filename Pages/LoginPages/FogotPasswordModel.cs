using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Components;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.SplitButtons;

namespace Northwind.Interface.Server.Pages.LoginPages
{
    public class FogotPasswordModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected EditContext RecoveryEmailContextObj { get; set; }
        protected FogotPasswordView FogotPasswordObj { get; set; } = new();
        protected SfProgressButton sfButtonRegister { get; set; }
        public bool EnableBtn { get; set; }
        [Inject]
        public CustomToastService ToastService { get; set; }

        protected override void OnInitialized()
        {
            RecoveryEmailContextObj = new EditContext(FogotPasswordObj);
        }
        public void SuccsessfullCaptcha(bool resultCaptcha)
        {
            EnableBtn = resultCaptcha;
        }
        protected async Task SubmitAsync()
        {
            if (RecoveryEmailContextObj.Validate())
            {
                var result = await (await httpClient.Client()).RecoveryPasswordAsync(FogotPasswordObj.Email);
                ToastService.ShowToast(localResource[result.Messages.First()], result.Succeeded ? MessageSeverity.Success : MessageSeverity.Error);
               await sfButtonRegister.EndProgressAsync();
            }
        }
    }
}
