using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.Grids;

namespace Northwind.Interface.Server.Pages
{
    public class SfShippersModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<ShipperReturnView> shipperGrid { get; set; }
        protected override void OnInitialized()
        {
            component.HeaderMessage = localResource["Title"];
        }
        protected async void OnActionBeginShipper(ActionEventArgs<ShipperReturnView> actionEvent)
        {
            try
            {
                if (shipperGrid != null && shipperGrid.removeDialog != null)
                    if (actionEvent.RequestType == Syncfusion.Blazor.Grids.Action.Delete && shipperGrid.removeDialog.Flag)
                    {
                        actionEvent.Cancel = true;  //Cancel default delete action.
                        await shipperGrid.removeDialog.ShowRemoveMessage($" {actionEvent.Data.CompanyName}");
                        shipperGrid.removeDialog.Flag = false;
                    }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.ToString());
            }
        }
    }
}
