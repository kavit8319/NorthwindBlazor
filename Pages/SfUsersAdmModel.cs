using Microsoft.AspNetCore.Components.Forms;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Northwind.Interface.Server.Pages
{
    public class SfUsersAdmModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<UserReturnView> customGrid;


        private ValidationMessageStore messageStore;

        protected override void OnInitialized()
        {

            component.HeaderMessage = localResource["Title"];
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // var result= await authentificationManager.Login(new UserReturn { EmailAddress = "kazakov1551983@gmail.com", Password = "123" });
                //   if(!result.Succeeded)
                //   {
                //       await ShowErrorMessage(result.Messages);
                //   }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }


        }
        protected async Task ValueChangeHandlerCustom(ChangeEventArgs<int, Role> args)
        {
            if (customGrid != null)
                await customGrid.CustomCombobxFilter("RoleName", args.Value, "RoleId");
        }

        public bool IsAdd { get; set; }

        #region Custom Grid
        public async void CustomActionBegin(ActionEventArgs<UserReturnView> Args)
        {

            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {

                IsAdd = false;
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                messageStore = new ValidationMessageStore(Args.EditContext);
                Args.EditContext.OnFieldChanged += CustomEditContext_OnFieldChanged;
                IsAdd = true;
            }

            if (customGrid != null && Args.RequestType.ToString() == "Delete" && customGrid.removeDialog != null && customGrid.removeDialog.Flag)
            {
                Args.Cancel = true;  //Cancel default delete action.
                await customGrid.removeDialog.ShowRemoveMessage($" {Args.Data.FirstName} {Args.Data.LastName}");
                customGrid.removeDialog.Flag = false;
            }
        }
      
        private async void CustomEditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            try
            {
                if (e.FieldIdentifier.FieldName == "EmailAddress")
                {

                    EditContext context = sender as EditContext;
                    var listRul = new List<ValidationResult>();
                    if (context != null && context.Model != null)
                    {
                        UserReturnView model = (UserReturnView)context.Model;
                        var user = new UserReturnView
                        {
                            EmailAddress = model.EmailAddress
                        };
                        var t = new ValidationContext(user);
                        Validator.TryValidateObject(user, t, listRul, true);
                        var res = listRul.FirstOrDefault(el => el.MemberNames.FirstOrDefault(el => el.Equals("EmailAddress")) != null);

                        if (res == null && messageStore != null)
                        {
                            messageStore.Clear();
                            var exist = await (await httpClient.Client()).EmailExistAsync(user.EmailAddress);
                            if (exist.Data && localResource != null)
                                messageStore.Add(e.FieldIdentifier, localResource["EmailExist"]);
                            else
                                messageStore.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.ToString());
            }
        }

        #endregion
    }
}
