using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.SplitButtons;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.Pages.LoginPages
{
    public class RegisterModel<TLocal> : BaseComponentModel<TLocal>
    {
        [CascadingParameter] public CenteredCardLayout centerCard { get; set; }
        [Inject] private IMapper map { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        protected SfProgressButton sfButtonRegister { get; set; }

        protected reCaptcha.Captcha captcha { get; set; } = new();
        public bool EnableRegisterBtn { get; set; }
        protected EditContext registerEditContext { get; set; }
        public UserReturnView RegisterUser { get; set; } = new UserReturnView() { RoleId = 2 };
        protected ValidationMessageStore message;

        protected override void OnInitialized()
        {
            centerCard.FormName = @localResource["Register_User"];
            registerEditContext = new EditContext(RegisterUser);
            registerEditContext.OnFieldChanged += RegisterEditContext_OnFieldChanged;
            message = new ValidationMessageStore(registerEditContext);

        }

        public void CaptchaSuccessful(bool res)
        {
            EnableRegisterBtn = res;
        }

        private async void RegisterEditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            var identifer = e.FieldIdentifier;
            if (identifer.FieldName.Contains("EmailAddress"))
            {
                var listRul = new List<ValidationResult>();
                UserReturnView model = (UserReturnView)e.FieldIdentifier.Model;
                var user = new UserReturnView
                {
                    EmailAddress = model.EmailAddress
                };
                var t = new ValidationContext(user);
                Validator.TryValidateObject(user, t, listRul, true);
                var res = listRul.FirstOrDefault(el =>
                    el.MemberNames.FirstOrDefault(el => el.Equals("EmailAddress")) != null);
                if (res == null)
                {
                    message.Clear();
                    var exist = await (await httpClient.Client()).EmailExistAsync(user.EmailAddress);
                    if (!exist.Succeeded && localResource != null)
                        message.Add(e.FieldIdentifier, localResource["EmailExist"]);
                    else
                        message.Clear();
                    registerEditContext.NotifyValidationStateChanged();
                }
            }
        }

        protected async void SubmitAsync()
        {
            if (registerEditContext.Validate())
            {
                try
                {
                    var res = await (await httpClient.Client()).AddAdminUserAsync(map.Map<UserReturn>(RegisterUser));
                    if (res.Succeeded)
                    {
                        await sfButtonRegister.EndProgressAsync();
                        NavigationManager.NavigateTo("/login?userCreated=true");
                    }
                }
                catch (Exception e)
                {
                   await component.ShowErrorMessage(e.Message);
                }
            }
        }
    }
}