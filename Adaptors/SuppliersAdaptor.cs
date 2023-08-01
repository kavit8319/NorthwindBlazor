using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class SuppliersAdaptor : BaseDataAdaptor
    {
        public SuppliersAdaptor(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string companyName = null;
            string contactName = null;
            string contactTitle = null;
            string address = null;
            string city = null;
            string region = null;
            string postalCode = null;
            string country = null;
            string phone = null;
            string homePage = null;
            Sort sort = null;

            if (dm.Sorted != null && dm.Sorted.Any())
                sort = dm.Sorted.FirstOrDefault();

            if (dm.Where != null && dm.Where.Any())
            {
                var filter = dm.Where.FirstOrDefault();
                if (filter != null)
                    foreach (WhereFilter predicate in filter.predicates)
                    {
                        switch (predicate.Field)
                        {
                            case nameof(SuppliersReturnView.CompanyName):
                                {
                                    companyName = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.ContactName):
                                {
                                    contactName = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.ContactTitle):
                                {
                                    contactTitle = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.Address):
                                {
                                    address = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.City):
                                {
                                    city = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.Region):
                                {
                                    region = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.PostalCode):
                                {
                                    postalCode = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.Country):
                                {
                                    country = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.Phone):
                                {
                                    phone = (string)predicate.value;
                                    break;
                                }
                            case nameof(SuppliersReturnView.HomePage):
                                {
                                    homePage = (string)predicate.value;
                                    break;
                                }
                        }
                    }
            }
            else
            {
                if (sort == null)
                    sort = new Sort() { Name = "CompanyName", Direction = "asc" };
            }
            IEnumerable<SupplierReturn> supplier = await ((await baseHttpClient.Client()).GetSuppliersPagingAsync(companyName, contactName, contactTitle, address, city, region, postalCode, country, phone, null, homePage, null,null, null, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : dm.Take > 0 ? (dm.Skip / dm.Take) + 1 : 1, dm.Take == 0 ? int.MaxValue : dm.Take));

            var count = supplier.Any() ? supplier.First().TotalRows : 0;
            var clientsMap = map?.Map<List<SuppliersReturnView>>(supplier.ToList());
            return dm.RequiresCounts ? new DataResult() { Result = clientsMap, Count = count.HasValue ? count.Value : 0 } : clientsMap;
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            dataManager.Headers.TryGetValue(Constans.Comfirm, out var strBoolComfirm);
            var result = await (await baseHttpClient.Client()).DeleteSupplierAsync(Convert.ToInt32(data), Convert.ToBoolean(strBoolComfirm));
            if (result == 0)
                throw new Exception("Error_RemoveSupplier_exist_product");
            return data;
        }
        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiUserReturn((SuppliersReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiUserReturn((SuppliersReturnView)data);
        }
        private async Task<SuppliersReturnView> ModifiUserReturn(SuppliersReturnView supplier)
        {
            try
            {
                var supplierIns = new SupplierReturn();
                if (supplier.Id > 0)
                {
                    supplierIns = map.Map<SupplierReturn>(supplier);
                    await (await baseHttpClient.Client()).ModifSupplierAsync(supplierIns);
                }
                else
                {
                    supplierIns = map.Map<SupplierReturn>(supplier);
                    supplierIns = await (await baseHttpClient.Client()).AddSupplierAsync(supplierIns);
                }
                return map.Map(supplierIns, supplier);
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
