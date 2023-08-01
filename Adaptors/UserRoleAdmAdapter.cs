using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class UserRoleAdmAdapter : BaseDataAdaptor
    {
        private IMemoryCache cash;
        public UserRoleAdmAdapter(BaseHttpClient http, IMemoryCache _cash) : base(http)
        {
            cash = _cash;
        }
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            var list = cash.Get("role") as List<Role>;
            if (list == null)
            {
                var result = (await (await baseHttpClient.Client()).GetUsersRolesAsync());
                cash.Set("role", result, Constans.MemoryCashMinute);
            }

            return dm.RequiresCounts ? new DataResult() { Result = list, Count =list!=null?list.Count:0 } : list;
        }
    }
}
