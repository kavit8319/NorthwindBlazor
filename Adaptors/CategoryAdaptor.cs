using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class CategoryAdaptor : BaseDataAdaptor
    {
        IMemoryCache memory;
        public CategoryAdaptor(BaseHttpClient http, IMapper mapper, IMemoryCache mc) : base(http, mapper)
        {
            memory = mc;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string categoryName = null;
            string categoryDescription = null;
            Sort sort = null;

            if (dm.Sorted != null && dm.Sorted.Any())
                sort = dm.Sorted.FirstOrDefault();

            if (dm.Where != null && dm.Where.Any())
            {
                var filter = dm.Where.FirstOrDefault();
                if (filter != null && filter.predicates != null)
                {
                    foreach (WhereFilter predicate in filter.predicates)
                    {
                        switch (predicate.Field)
                        {
                            case nameof(CategoryView.CategoryName):
                                {
                                    categoryName = (string)predicate.value;
                                    break;
                                }
                            case nameof(CategoryView.Description):
                                {
                                    categoryDescription = (string)predicate.value;
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    switch (filter?.Field)
                    {
                        case nameof(CategoryView.CategoryName):
                            {
                                categoryName = (string)filter.value;
                                break;
                            }
                        case nameof(CategoryView.Description):
                            {
                                categoryDescription = (string)filter.value;
                                break;
                            }
                    }
                }
            }

            IEnumerable<CategoryView> clients = new List<CategoryView>();
            //if (categoryDescription == null && categoryName == null && memory?.Get<List<CategoryView>>(Constans.Category) != null)
            //{
            //    clients = memory?.Get<List<CategoryView>>(Constans.Category);
            //}
            //else if (categoryDescription == null && categoryName == null)
            //{
            clients = await ((await baseHttpClient.Client()).GetCategorysPagingAsync(categoryName, categoryDescription, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : dm.Take > 0 ? (dm.Skip / dm.Take) + 1 : 1, dm.Take == 0 ? int.MaxValue : dm.Take));
            //    memory?.Set(Constans.Category, clients);
            //}
            var count = clients?.First().TotalRows;

            var clientsMap = map?.Map<List<CategoryReturnView>>(clients);
            return dm.RequiresCounts ? new DataResult() { Result = clientsMap, Count = count.HasValue ? count.Value : 0 } : clientsMap;
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            dataManager.Headers.TryGetValue(Constans.Comfirm, out var strBoolComfirm);
            var result = await (await baseHttpClient.Client()).DeleteCategoryAsync(Convert.ToInt32(data), Convert.ToBoolean(strBoolComfirm));
            if (result == 0)
                throw new Exception($"Id:{data}",new Exception("Error_RemoveCategory_exist_product"));
            return data;
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiUserReturn((CategoryReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiUserReturn((CategoryReturnView)data);
        }
        private async Task<CategoryReturnView> ModifiUserReturn(CategoryReturnView categoryNew)
        {
            try
            {
                var category = new CategoryView();

                if (categoryNew.Id > 0)
                {
                    category = map.Map<CategoryView>(categoryNew);
                    await (await baseHttpClient.Client()).ModifiCategoryAsync(category);
                }
                else
                {
                    category = map.Map<CategoryView>(categoryNew);
                    category = await (await baseHttpClient.Client()).AddCategoryAsync(category);
                }
                return map.Map(category,categoryNew);
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
