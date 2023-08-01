using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace Northwind.Interface.Server.Adaptors
{
    public class BaseDataAdaptor : DataAdaptor
    {
        protected IServiceCollectionProvider config { get; set; }
        protected BaseHttpClient baseHttpClient { get; set; }

        protected IMapper map { get; set; } = default;

        public BaseDataAdaptor(BaseHttpClient _httpClient, IMapper mapper)
        {
            map=mapper;
            baseHttpClient = _httpClient;
        }
        public BaseDataAdaptor(BaseHttpClient _httpClient)
        {
           baseHttpClient = _httpClient;
        }
        public string GetSortDirection(Syncfusion.Blazor.Data.Sort sort)
        {
            if (sort != null && sort.Direction.Contains("desc"))
                return "desc";
            return sort == null ? null : "asc";
        }
        protected static Sort SetSortField(Sort sort, WhereFilter predicate)
        {
            if (predicate.value != null&&sort==null)
                sort = new Sort() { Name = predicate.Field, Direction = "asc" };
            return sort;
        }
       
    }

}