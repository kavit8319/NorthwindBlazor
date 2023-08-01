using Microsoft.Extensions.Caching.Memory;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class CustomerTitleAdaptor:BaseDataAdaptor
    {
        private readonly IMemoryCache memory;
        public CustomerTitleAdaptor(BaseHttpClient http, IMemoryCache _memory) : base(http)
        {
            memory = _memory;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {
            var listTitle = memory.Get<List<TitleReturn>>("titleCustomer");
            if (listTitle != null) return listTitle;
            var result = await (await baseHttpClient.Client()).GetTitlesAsync("customer");
            listTitle = result.ToList();
            memory.Set("titleCustomer", listTitle);
            return listTitle;
        }
    }
}
