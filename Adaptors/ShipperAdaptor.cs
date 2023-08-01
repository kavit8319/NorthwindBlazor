using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class ShipperAdaptor : BaseDataAdaptor

    {
        public ShipperAdaptor(BaseHttpClient http,IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            var res = await (await baseHttpClient.Client()).SelectAllShippersAsync();
            IEnumerable<ShipperReturnView> ret = map.Map<List<ShipperReturnView>>(res);
            var count = res.Any() ? res.Count : 0;

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                // Sorting
                ret = DataOperations.PerformSorting(ret, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0)
            {
                // Filtering
                ret = DataOperations.PerformFiltering(ret, dm.Where, dm.Where[0].Operator);
            }
            if (dm.Skip != 0)
            {
                //Paging
                ret = DataOperations.PerformSkip(ret, dm.Skip);
            }
            if (dm.Take != 0)
            {
                ret = DataOperations.PerformTake(ret, dm.Take);
            }
            return dm.RequiresCounts ? new Syncfusion.Blazor.Data.DataResult() { Result = ret.ToList().OrderByDescending(el => el.Id), Count = count } : ret.ToList();
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            var tt= await Modifi((ShipperReturnView)data);
            return tt;
        }

        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await Modifi((ShipperReturnView)data);
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            await (await baseHttpClient.Client()).DeleteShipperAsync((int)data);
            return data;
        }

        private async Task<ShipperReturnView> Modifi(ShipperReturnView shipper)
        {
            try
            {
                ShipperReturn shipIns=new ShipperReturn();
                if (shipper.Id > 0)
                {
                    shipIns = map?.Map<ShipperReturn>(shipper);
                    await (await baseHttpClient.Client()).EditShipperAsync(shipIns);
                    return map?.Map<ShipperReturnView>(shipIns);
                }
                shipIns = map?.Map<ShipperReturn>(shipper);
                shipIns = await (await baseHttpClient.Client()).AddShipperAsync(shipIns);
                return map?.Map(shipIns,shipper);
                // ShowMessage($"{userIns.FirstName} {userIns.LastName}", !(userIns.UserID > 0));
            }
            catch (Exception)
            {
                // ShowErrorMessage(ex);
            }

            return null;
        }
    }
}
