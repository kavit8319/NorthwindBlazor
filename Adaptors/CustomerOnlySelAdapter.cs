using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class CustomerOnlySelAdapter:BaseDataAdaptor
    {
        public CustomerOnlySelAdapter(BaseHttpClient http) : base(http)
        {
        }
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            try
            {
                IEnumerable<CustomerReturn> customers = await ((await baseHttpClient.Client()).SelectCustomersPagingAsync(null, null, null, null,null ,null,null, null, null, null, null, null, null,null, 1, Int32.MaxValue, null));

                var count = customers.Any() ? customers.First().TotalRows : 0;
                var clientsMap = map?.Map<List<CustomerReturnView>>(customers.ToList());
                return dm.RequiresCounts ? new DataResult() { Result = clientsMap, Count = count.HasValue ? count.Value : 0 } : clientsMap;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
