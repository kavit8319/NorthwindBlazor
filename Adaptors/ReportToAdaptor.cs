using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class ReportToAdaptor:BaseDataAdaptor
    {

        public ReportToAdaptor(BaseHttpClient http) : base(http)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            int? employeeId = null;
            if (dm.Params != null && dm.Params.Count > 0)
            {
               var param= dm.Params.First();
               employeeId = Convert.ToInt32(param.Value);
            }
            var result = await (await baseHttpClient.Client()).GetReportToAsync(employeeId);
            return result;
        }
    }
}
