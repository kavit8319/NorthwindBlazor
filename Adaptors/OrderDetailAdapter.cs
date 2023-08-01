using AutoMapper;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Pages.TestPages;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;

namespace Northwind.Interface.Server.Adaptors
{
    public class OrderDetailAdapter : BaseDataAdaptor
    {
        public OrderDetailAdapter(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {

            string productName = null;
            int? unitInStorc = null;
            int? quantity = null;
            double? unitPrice = null;
            float? discont = null;
            Sort sort = null;

            if (dm.Sorted != null && dm.Sorted.Any())
                sort = dm.Sorted.FirstOrDefault();

            if (dm.Where != null)
            {
                var filter = dm.Where.FirstOrDefault();
                if (filter != null && filter.predicates != null)
                {
                    foreach (WhereFilter predicate in filter.predicates)
                    {
                        switch (predicate.Field)
                        {
                            case nameof(OrderDetailReturn.ProductName):
                                {
                                    productName = (string)predicate.value;
                                    break;
                                }
                            case nameof(OrderDetailReturn.UnitsInStock):
                                {
                                    unitInStorc = Convert.ToInt32(predicate.value);
                                    break;
                                }
                            case nameof(OrderDetailReturn.Quantity):
                                {
                                    quantity = Convert.ToInt32(predicate.value);
                                    break;
                                }
                            case nameof(OrderDetailReturn.UnitPrice):
                                {
                                    unitPrice = Convert.ToDouble(predicate.value);
                                    break;
                                }
                            case nameof(OrderDetailReturn.Discount):
                                {
                                    discont = (float)predicate.value;
                                    break;
                                }
                        }
                    }
                }
                var ordId = dm.Where.First().value;
               
                var orderId = (ordId is int ? (int)ordId : 0);
                return await LoadDate(dm.RequiresCounts, productName, unitInStorc, quantity, unitPrice, discont, orderId, sort,dm.Aggregates);
            }
            
            if (dm.Params != null && dm.Params.Any())
            {

                if (dm.Params.TryGetValue(Constans.OrderId, out var Id))
                {
                    if (Id is int orderId)
                    {
                        return await LoadDate(dm.RequiresCounts, null, null, null, null, null, orderId,sort,dm.Aggregates);
                    }
                }
            }
            
            return dm.RequiresCounts
                ? new DataResult() { Result = new List<OrderDetailReturnView>(), Count = 0 }
                : null;

        }

        private async Task<object> LoadDate(bool requestCount, string productName, int? unitInStorc, int? quantity,
            double? unitPrice, float? discont, int orderId, Sort sort,List<Aggregate>aggregate)
        {
            var result = await (await baseHttpClient.Client()).SelectOrdersDetailAsync(productName, null, unitInStorc,
                quantity, unitPrice, discont,null, orderId, null, sort?.Name, GetSortDirection(sort));

            var clientsMap = map?.Map<List<OrderDetailReturnView>>(result);
           var res = DataUtil.PerformAggregation(clientsMap, aggregate);
            return requestCount
                ? new DataResult() { Result = clientsMap,Aggregates= new Dictionary<string, object> { { "TotalSumm - sum", clientsMap.Sum(el => el.TotalSumm) } }, Count = clientsMap.Count }
                : clientsMap;
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiOrdeerDetailReturn((OrderDetailReturnView)data);
        }

        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiOrdeerDetailReturn((OrderDetailReturnView)data);
        }

        private async Task<OrderDetailReturnView> ModifiOrdeerDetailReturn(OrderDetailReturnView orderDetail)
        {
            try
            {
                var orderDetailIns = new OrderDetailReturn();
                if (!orderDetail.IsNew)
                {
                    orderDetailIns = map.Map<OrderDetailReturn>(orderDetail);
                    var str=JsonConvert.SerializeObject(orderDetailIns);
                  var result=  await (await baseHttpClient.Client()).EditOrderDetailAsync(orderDetailIns);
                   orderDetailIns.TotalSumm= result.TotalSumm;
                }
                else
                {
                    orderDetailIns = map.Map<OrderDetailReturn>(orderDetail);
                    orderDetailIns = await (await baseHttpClient.Client()).AddOrderDetailAsync(orderDetailIns);
                }
                return map.Map(orderDetailIns, orderDetail);
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            dataManager.Headers.TryGetValue(Constans.ProductId, out string productId);
            await (await baseHttpClient.Client()).DeleteOrderDetailAsync(Convert.ToInt32(data), Convert.ToInt32(productId));
            return data;
        }
    }
}
