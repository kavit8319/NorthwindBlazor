using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class TitlesAdaprter:BaseDataAdaptor
    {
        private readonly IMemoryCache memory;
        public TitlesAdaprter(BaseHttpClient http,IMemoryCache memory_) : base(http)
        {
            memory = memory_;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {
           var listTitle= memory.Get<List<TitleReturn>>("title");
           if (listTitle != null&&listTitle.Count>0) return listTitle;
           var result= await (await baseHttpClient.Client()).GetTitlesAsync("employee");
           listTitle = result.ToList();
           memory.Set("title", listTitle);
           return listTitle;
        }
    }
}
