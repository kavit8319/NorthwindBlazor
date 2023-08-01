using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Net;

namespace Northwind.Interface.Server.Adaptors
{

    public class UserAdmAdaptor : BaseDataAdaptor
    {
        public UserAdmAdaptor(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {

        }
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            string firstName = null;
            string lastName = null;
            string email = null;
            int? roleId = null;
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
                            case nameof(UserReturnView.FirstName):
                                {
                                    firstName = (string)predicate.value;
                                    break;
                                }
                            case nameof(UserReturnView.LastName):
                                {
                                    lastName = (string)predicate.value;
                                    break;
                                }
                            case nameof(UserReturnView.EmailAddress):
                                {
                                    email = (string)predicate.value;
                                    break;
                                }
                            case nameof(UserReturnView.RoleName):
                                {
                                    roleId = (int?)predicate.value == 0 ? null : (int?)predicate.value;
                                    break;
                                }
                        }
                    }
            }
          
   var clients = await ((await baseHttpClient.Client()).GetUsersPagingAsync(firstName, lastName, roleId, email, sort?.Name, GetSortDirection(sort), dm.Skip == 0 ? 1 : (dm.Skip / dm.Take) + 1, dm.Take));
            var count = clients.Data.First().TotalRows;

            var clientsMap = map.Map<List<UserReturn>, List<UserReturnView>>(clients.Data.ToList());
            return dm.RequiresCounts ? new DataResult() { Result = clientsMap, Count = count.HasValue ? count.Value : 0 } : clientsMap;
           
           
        
           
         
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            await (await baseHttpClient.Client()).UsersDELETEAsync(Convert.ToInt32(data));
            return data;
        }
        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            return await ModifiUserReturn((UserReturnView)data); ;
        }
        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            return await ModifiUserReturn((UserReturnView)data);
        }
        private async Task<UserReturnView> ModifiUserReturn(UserReturnView userNew)
        {
            try
            {
                var userIns = new UserReturn();

                if (userNew.Id > 0)
                {
                    userIns = map.Map(userNew, userIns);
                    if (userIns != null)
                        await (await baseHttpClient.Client()).EditAdminUserAsync(userIns);
                }
                else
                {
                    
                    userIns = map.Map(userNew, userIns);
                    userIns.EmailVerified = true;
                    var userReturn = await (await baseHttpClient.Client()).AddAdminUserAsync(userIns);
                    if (userReturn.Succeeded)
                        userIns.UserID = userReturn.Data.UserID;
                }
                return map.Map(userIns, userNew);
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