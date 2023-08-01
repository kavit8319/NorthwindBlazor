using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Sort = Syncfusion.Blazor.Data.Sort;

namespace Northwind.Interface.Server.Adaptors
{
    public class EmployeesAdaptor : BaseDataAdaptor
    {
      
        private IEnumerable<EmployeeReturnView> DataSource;
        public EmployeesAdaptor(BaseHttpClient httpClient, IMapper mapper) : base( httpClient, mapper)
        {
            DataSource = new List<EmployeeReturnView>();
        }



        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            int? ReportTo = default;
            string filterType = null;
            string collName = null;
            if (dm.Where != null && dm.Where.Count > 0)
            {
                var pridicate = dm.Where.First();
                collName = pridicate.Field;
                if (collName == "ReportsTo")
                {
                    filterType = pridicate.Operator;
                    if (pridicate.value is null)
                        ReportTo = -1;
                    else
                        ReportTo = Convert.ToInt32(pridicate.value);
                }
            }

            IEnumerable<EmployeeReturn> employee = await ((await baseHttpClient.Client()).GetEmployeesPagingAsync(
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null, null, null,
                    ReportTo, null,
                    null, null,
                    null, collName, null, dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1, Convert.ToInt32((int.MaxValue / (dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1))), filterType));
            DataSource = map?.Map<IEnumerable<EmployeeReturnView>>(employee);

            if (dm.Search != null && dm.Search.Count > 0)
            {
                // Searching
                DataSource = DataOperations.PerformSearching(DataSource, dm.Search);
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                // Sorting
                DataSource = DataOperations.PerformSorting(DataSource, dm.Sorted);
            }
            //if (dm.Where != null && dm.Where.Count > 0)
            //{
            //    // Filtering
            //    DataSource = DataOperations.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            //}
            int count = 0;
            if (DataSource != null && DataSource.Any())
            {
                count = DataSource.Where(el =>ReportTo==-1?el.ReportsTo == null:el.ReportsTo!=null).Count();
            }
            if (dm.Skip != 0)
            {
                //Paging
                DataSource = DataOperations.PerformSkip(DataSource, dm.Skip);
            }
            //if (dm.Take != 0)
            //{
            //    DataSource = DataOperations.PerformTake(DataSource, dm.Take);
            //}
            return dm.RequiresCounts ? new DataResult() { Result = DataSource, Count = count } : (object)DataSource;
        }

        private async Task<object> FilterEmployees(DataManagerRequest dm)
        {
            string firstname = null;
            string lastName = null;
            int? titleId = null;
            string titleOfCourtesyl = null;
            string city = null;
            string address = null;
            string postalCode = null;
            string country = null;
            DateTime? birthDate = default;
            DateTime? hireDate = default;
            int? reportTo = null;
            string note = null;
            string filterType = null;

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
                            case nameof(EmployeeReturn.ReportsTo):
                                {
                                    reportTo = Convert.ToInt32(predicate.value);
                                    break;
                                }
                            case nameof(EmployeeReturn.FirstName):
                                {
                                    firstname = (string)predicate.value;
                                    break;
                                }
                            case nameof(EmployeeReturn.LastName):
                                {
                                    lastName = (string)predicate.value;
                                    break;
                                }
                            case nameof(EmployeeReturn.TitleId):
                                {
                                    titleId = Convert.ToInt32(predicate.value);
                                    break;
                                }
                            case nameof(EmployeeReturn.TitleOfCourtesy):
                                {
                                    titleOfCourtesyl = (string)predicate.value;

                                    break;
                                }
                            case nameof(EmployeeReturn.BirthDate):
                                {
                                    birthDate = Convert.ToDateTime(predicate.value);

                                    break;
                                }
                            case nameof(EmployeeReturn.HireDate):
                                {
                                    hireDate = Convert.ToDateTime(predicate.value);
                                    break;
                                }
                            case nameof(EmployeeReturn.Country):
                                {
                                    country = (string)predicate.value;
                                    break;
                                }
                            case nameof(EmployeeReturn.City):
                                {
                                    city = Convert.ToString(predicate.value);
                                    break;
                                }
                            case nameof(EmployeeReturn.PostalCode):
                                {
                                    postalCode = Convert.ToString(predicate.value);
                                    break;
                                }
                        }
                    }
                }
            }

            try
            {
                IEnumerable<EmployeeReturn> employee = await ((await baseHttpClient.Client()).GetEmployeesPagingAsync(firstname, lastName,
                     titleOfCourtesyl, birthDate, hireDate, address, city, postalCode, country, null, null, titleId, note, null, null, null, null,
                    null, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1, dm.Take, filterType));
                var count = employee.Any() ? employee.First().TotalRows : 0;
                var products = map?.Map<IEnumerable<EmployeeReturnView>>(employee);
                products.First(el => el.Id == 1).isParent = false;
                products.First(el => el.Id == 2).isParent = true;
                //products.First(el => el.Id == 3).isParent = false;
                //products.First(el => el.Id == 4).isParent = false;
                //products.First(el => el.Id == 5).isParent = true;
                //products.First(el => el.Id == 6).isParent = false;
                //products.First(el => el.Id == 7).isParent = false;
                //products.First(el => el.Id == 8).isParent = false;
                //products.First(el => el.Id == 9).isParent = false;


                if (dm.Where != null && dm.Where.Count > 0)
                {
                    // Filtering
                    products = DataOperations.PerformFiltering(products, dm.Where, dm.Where[0].Operator);
                }


                return dm.RequiresCounts ? new DataResult() { Result = products, Count = products.Count() } : products;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }

            return null;
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiReturn((EmployeeReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiReturn((EmployeeReturnView)data);
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {

            await (await baseHttpClient.Client()).DeleteEmployeeAsync(Convert.ToInt32(data));
            return data;
        }
        private async Task<EmployeeReturnView> ModifiReturn(EmployeeReturnView el)
        {
            try
            {
                var elNew = new EmployeeReturn();

                if (el.Id > 0)
                {
                    elNew = map?.Map<EmployeeReturn>(el);
                    await (await baseHttpClient.Client()).EditEmployeeAsync(elNew);
                }
                else
                {
                    elNew = map?.Map<EmployeeReturn>(el);
                    elNew = await (await baseHttpClient.Client()).AddEmployeeAsync(elNew);
                }
                return map?.Map(elNew, el);
                // ShowMessage($"{userIns.FirstName} {userIns.LastName}", !(userIns.UserID > 0));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
