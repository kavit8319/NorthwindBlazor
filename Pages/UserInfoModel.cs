using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor.SplitButtons;

namespace Northwind.Interface.Server.Pages
{
    public class UserInfoModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected UserReturnView UserReturn { get; set; } = new();
        protected EditContext EditContext { get; set; }
        protected SfProgressButton sfButtonRegister { get; set; }
        [Inject] Authentification.IAuthenticationManager AuthenticationManager { get; set; }
        [Inject] protected IMapper mapper { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
               component.HeaderMessage = localResource["HeaderUserInfo"];
                
                var user = await AuthenticationManager.CurrentUser();
                var res = await (await httpClient.Client()).GetUserByEmailAsync(user.Identity.Name);
                res.Data.Password = null;
                UserReturn = mapper.Map<UserReturnView>(res.Data);
                UserReturn.Password=UserReturn.PasswordConfirm = null;
                EditContext = new EditContext(UserReturn);
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }
        }
       

        protected async Task Submit()
        {
            try
            {
                var user = mapper.Map<UserReturn>(UserReturn);
                var res = await (await httpClient.Client()).ChangePersonalInfoAsync(user);
                await sfButtonRegister.EndProgressAsync();
                if (res.Succeeded)
                    await component.ShowMessage("UserUpdated", true);
                else
                    await component.ShowErrorMessage("UserUpdatedError");
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }
        }
    }
}