using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class ProductsAdaprtor : BaseDataAdaptor
    {
        public ProductsAdaprtor(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string productName = null;
            int? categoryId = null;
            int? supplierId = null;
            string quantityPerUnit = null;
            double? unitPrice = null;
            int? unitsInStock = null;
            int? unitsOnOrder = null;
            int? reorderLevel = null;
            string filterType = null;
            bool? discontinued = null;
            string productIdsExclude = null;
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
                        filterType = predicate.Operator;
                        if (sort == null)
                            sort = new Sort() { Name = predicate.Field, Direction = "asc" };
                        switch (predicate.Field)
                        {
                            case nameof(ProductReturn.ProductName):
                                productName = (string)predicate.value;
                                break;
                            case nameof(ProductReturn.SupCompanyName):
                                supplierId = int.Parse((string)predicate.value);
                                sort = new Sort() { Name = "SupplierId", Direction = "asc" };
                                break;
                            case nameof(ProductReturn.CategoryName):
                                categoryId = int.Parse((string)predicate.value);
                                sort = new Sort() { Name = "CategoryId", Direction = "asc" };
                                break;
                            case nameof(ProductReturn.QuantityPerUnit):
                                quantityPerUnit = (string)predicate.value;
                                break;
                            case nameof(ProductReturn.UnitPrice):
                                unitPrice = Convert.ToDouble(predicate.value);
                                break;
                            case nameof(ProductReturn.UnitsInStock):
                                unitsInStock = Convert.ToInt32(predicate.value);
                                break;
                            case nameof(ProductReturn.UnitsOnOrder):
                                unitsOnOrder = Convert.ToInt32(predicate.value);
                                break;
                            case nameof(ProductReturn.ReorderLevel):
                                reorderLevel = Convert.ToInt32(predicate.value);
                                break;
                            case nameof(ProductReturn.Discontinued):
                                discontinued = Convert.ToBoolean(predicate.value);
                                break;
                        }
                    }
                }

                if (filter != null && !string.IsNullOrEmpty(filter.Field))
                {
                    productName = Convert.ToString(filter.value);
                    filterType = filter.Operator;
                    sort = new Sort() { Name = filter.Field, Direction = "asc" };
                }

            }

            if (dm.Params != null)
            {
                if (dm.Params.TryGetValue(Constans.ProductIds, out var str))
                    productIdsExclude = Convert.ToString(str);
            }
            try
            {
                IEnumerable<ProductReturn> product = await ((await baseHttpClient.Client()).GetSelectProductPagingAsync(productName, supplierId, categoryId, null, null, quantityPerUnit, unitPrice, unitsInStock, unitsOnOrder, reorderLevel, discontinued, productIdsExclude, null, null, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1, dm.Take == 0 ? int.MaxValue : dm.Take, filterType));
                var count = product.Any() ? product.First().TotalRows : 0;
                var products = map.Map<List<ProductReturnView>>(product);
                return dm.RequiresCounts ? new DataResult() { Result = products, Count = count ?? 0 } : products;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
            return null;
        }
        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiReturn((ProductReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiReturn((ProductReturnView)data);
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            await (await baseHttpClient.Client()).DeleteProductAsync(Convert.ToInt32(data));
            return data;
        }
        private async Task<ProductReturnView> ModifiReturn(ProductReturnView el)
        {
            try
            {
                var elNew = new ProductReturn();

                if (el.Id > 0)
                {
                    elNew = map.Map<ProductReturn>(el);
                    await (await baseHttpClient.Client()).EditProductAsync(elNew);
                }
                else
                {
                    elNew = map.Map<ProductReturn>(el);
                    elNew = await (await baseHttpClient.Client()).AddProductAsync(elNew);
                }
                return map.Map(elNew, el);
                // ShowMessage($"{userIns.FirstName} {userIns.LastName}", !(userIns.UserID > 0));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
