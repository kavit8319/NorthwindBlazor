using AutoMapper;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using static System.Convert;

namespace Northwind.Interface.Server.Adaptors
{
    public class OrdersAdaptor : BaseDataAdaptor
    {
        public OrdersAdaptor(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string filterType = null;
            string companyName = null;
            string contactName = null;
            string emplFullName = null;
            double? freight = null;
            string shipName = null;
            string shipAddress = null;
            string shipCountry = null;
            string shipCity = null;
            string shipPostalCode = null;
            double? totalSumm = null;
            DateTime? orderDate = null;
            DateTime? requiredDate = null;
            DateTime? shippedDate = null;

            Sort sort = null;

            if (dm.Sorted != null && dm.Sorted.Any())
                sort = dm.Sorted.FirstOrDefault();

            if (dm.Where != null && dm.Where.Any())
            {
                var filter = dm.Where.FirstOrDefault();
                if (filter != null && filter.predicates != null)
                {
                   
                    foreach (var predicate in filter.predicates)
                    {

                        if (filter.predicates.Any(el => el.value != null))
                            filterType = predicate.Operator;

                        switch (predicate.Field)
                        {
                            case nameof(OrderReturn.CompanyName):
                                companyName = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.ContactName):
                                contactName = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.EmplFullName):
                                emplFullName = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.Freight):
                                freight = ToDouble(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.ShipName):
                                shipName = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.ShipAddress):
                                shipAddress = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.City):
                                shipCity = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.Country):
                                shipCountry = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.PostalCode):
                                shipPostalCode = Convert.ToString(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.TotalSumm):
                                totalSumm = ToDouble(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.OrderDate):
                                orderDate = ToDateTime(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.ShippedDate):
                                shippedDate = ToDateTime(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                            case nameof(OrderReturn.RequiredDate):
                                requiredDate = ToDateTime(predicate.value);
                                sort = SetSortField(sort, predicate);
                                break;
                        }
                    }
                }
            }
            try
            {
                IEnumerable<OrderReturn> orders = await ((await baseHttpClient.Client()).SelectAllOrdersAsync(null, null, null, null, null, companyName, contactName, emplFullName, freight, shipName, shipAddress, shipCountry, shipCity, shipPostalCode, null, null, totalSumm, orderDate, requiredDate, shippedDate, null, null, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1, dm.Take, filterType));
                var count = orders.Any() ? orders.First().TotalRows : 0;
                var products = map?.Map<List<OrderReturnView>>(orders);
                return dm.RequiresCounts ? new DataResult() { Result = products, Count = count ?? 0 } : products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

       

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            try
            {
                await ((await baseHttpClient.Client()).DeleteOrderAsync(ToInt32(data)));
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiReturn((OrderReturnView)data);
        }

        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiReturn((OrderReturnView)data);
        }

        private async Task<OrderReturnView> ModifiReturn(OrderReturnView el)
        {
            try
            {
                OrderReturn elNew;

                if (el.Id > 0)
                {
                    elNew = map.Map<OrderReturn>(el);
                    // var str = JsonConvert.SerializeObject(elNew);
                    await (await baseHttpClient.Client()).EditOrderAsync(elNew);
                }
                else
                {
                    elNew = map.Map<OrderReturn>(el);

                    elNew = await (await baseHttpClient.Client()).AddOrderAsync(elNew);

                }
                return map.Map(elNew, el);
                // ShowMessage($"{userIns.FirstName} {userIns.LastName}", !(userIns.UserID > 0));
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
