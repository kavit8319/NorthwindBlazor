using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Resources.Localization;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.Grids;
using System.ComponentModel;
using Action = Syncfusion.Blazor.Grids.Action;

namespace Northwind.Interface.Server.Pages
{
    public class SfSuppliersModel<TLocale> : BaseComponentModel<TLocale>
    {
        public CustomGridAddEditDel<SuppliersReturnView> SuppliersGrid { get; set; }

        protected Dictionary<string, string> HeaderRemove;
        protected CountryCityPostCode<SuppliersReturnView> ddlCityPostCode { get; set; }
        public RemoveDialog<SuppliersReturnView> removeDialog { get; set; }

        protected override void OnInitialized()
        {
            component.HeaderMessage = localResource["Title"];
        }
        public async Task CustomActionBeginSupplier(ActionEventArgs<SuppliersReturnView> Args)
        {
            try
            {
                if (SuppliersGrid != null && Args.RequestType == Action.Delete && SuppliersGrid.removeDialog != null && SuppliersGrid.removeDialog.Flag)
                {
                    if (!removeReletionDate)
                    {
                        Args.Cancel = true; //Cancel default delete action.
                        HeaderRemove = new Dictionary<string, string>() { { Constans.Comfirm, "false" } };
                        await SuppliersGrid.removeDialog.ShowRemoveMessage($"{Args.Data.CompanyName}");
                        SuppliersGrid.removeDialog.Flag = false;

                    }

                    removeReletionDate = false;
                }

                if (Args.RequestType == Action.Save)
                {
                    Args.Data.Country = Args.Data.CountryCode != null ? ddlCityPostCode.DdlCountry.GetDataByValue(Args.Data.CountryCode).Name : null;
                }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.ToString());
            }
        }

        private bool removeReletionDate;
        public async Task RemoveReletionProduct(Exception ex)
        {
            HeaderRemove = new Dictionary<string, string>() { { Constans.Comfirm, "true" } };
            removeDialog.Flag = false;
            removeReletionDate = true;
            await removeDialog.ShowRemoveMessage(SuppliersGrid.GlobalStringLocalizer[ex.Message]);

        }
    }
}
