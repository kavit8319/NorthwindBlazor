using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class CustomersAdapter : BaseDataAdaptor
    {
        public CustomersAdapter(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string companyName = null;
            string contactName = null;
            int? customerTitleId = null;
            string address = null;
            string city = null;
            bool? isVip = null;
            string postalCode = null;
            string country = null;
            string phone = null;
            Sort sort = null;

            if (dm.Sorted != null && dm.Sorted.Any())
                sort = dm.Sorted.FirstOrDefault();
            else
                sort = new Sort() { Name = "CompanyName", Direction = "asc" };

            if (dm.Where != null && dm.Where.Any())
            {
                var filter = dm.Where.FirstOrDefault();
                if (filter != null)
                    foreach (WhereFilter predicate in filter.predicates)
                    {
                        switch (predicate.Field)
                        {
                            case nameof(CustomerReturnView.CompanyName):
                                companyName = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.ContactName):
                                contactName = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.CustomerTitle):
                                customerTitleId = Convert.ToInt32(predicate.value);
                                if (customerTitleId == 0)
                                    customerTitleId = null;
                                break;
                            case nameof(CustomerReturnView.Address):
                                address = Convert.ToString(predicate.value);
                                break;
                            case nameof(CustomerReturnView.City):
                                city = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.PostalCode):
                                postalCode = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.Country):
                                country = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.Phone):
                                phone = (string)predicate.value;
                                break;
                            case nameof(CustomerReturnView.IsVip):
                                isVip = Convert.ToBoolean(predicate.value);
                                break;
                        }
                    }
            }

            IEnumerable<CustomerReturn> customers = await ((await baseHttpClient.Client()).SelectCustomersPagingAsync(companyName, null, customerTitleId, contactName, address, city, postalCode, country, phone, isVip, null, null, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : dm.Take > 0 ? (dm.Skip / dm.Take) + 1 : 1, dm.Take == 0 ? int.MaxValue : dm.Take, null));

            var count = customers.Any() ? customers.First().TotalRows : 0;
            var clientsMap = map?.Map<List<CustomerReturnView>>(customers.ToList());
            return dm.RequiresCounts ? new DataResult() { Result = clientsMap, Count = count.HasValue ? count.Value : 0 } : clientsMap;
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            await (await baseHttpClient.Client()).DeleteCustomersAsync(Convert.ToInt32(data));
            return data;
        }
        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiUserReturn((CustomerReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiUserReturn((CustomerReturnView)data);
        }
        private async Task<CustomerReturnView> ModifiUserReturn(CustomerReturnView empl)
        {
            try
            {
                var emplIns = new CustomerReturn();
                if (empl.Id > 0)
                {
                    emplIns = map?.Map<CustomerReturn>(empl);
                    await (await baseHttpClient.Client()).EditCustomerAsync(emplIns);
                }
                else
                {
                    emplIns = map?.Map<CustomerReturn>(empl);
                    emplIns = await (await baseHttpClient.Client()).AddCustomerAsync(emplIns);
                }
                return map?.Map(emplIns, empl);
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
