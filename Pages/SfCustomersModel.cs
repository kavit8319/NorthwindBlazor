using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Action = Syncfusion.Blazor.Grids.Action;


namespace Northwind.Interface.Server.Pages
{
    public class SfCustomersModel<TLocal>:BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<CustomerReturnView> customerGrid;

        protected CountryCityPostCode<CustomerReturnView> customerCountryCityPostCode { get; set; }

        public Country selCountry { get; set; } = new Country();

        protected DialogSettings dialogSettings = new DialogSettings() { MinHeight = "450", Height = "450" };

        protected override void OnInitialized()
        {
            component.HeaderMessage = localResource["Title"];
        }

        public async void CustomActionBeginCustomer(ActionEventArgs<CustomerReturnView> Args)
        {

            if (Args.RequestType == Action.BeginEdit)
            {
                selCountry = new Country() { Code = Args.Data.Country, Name = Args.Data.Country };
                
            }

            if (Args.RequestType == Action.Add)
            {
                selCountry = null;
               
            }
            if (customerGrid != null && Args.RequestType == Action.Delete && customerGrid.removeDialog != null && customerGrid.removeDialog.Flag)
            {
                Args.Cancel = true;  //Cancel default delete action.
                await customerGrid.removeDialog.ShowRemoveMessage($"{Args.Data.CompanyName}");
                customerGrid.removeDialog.Flag = false;
            }

            if (Args.RequestType == Action.Save && customerCountryCityPostCode.DdlCountry != null)
            {
               // Args.Data.Country = Args.Data.Country != null ?customerCountryCityPostCode.ddlCountry.GetDataByValue(Args.Data.Country).Name : null;
            }
        }
        
        protected async Task ValueChangeHandlerCustom(ChangeEventArgs<int, TitleReturn> args)
        {
            if (customerGrid != null)
                await customerGrid.CustomCombobxFilter("CustomerTitle", args.Value, "CustomerTitleId");
        }
    }
}
