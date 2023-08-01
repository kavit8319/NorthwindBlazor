using AutoMapper;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class EmployeesOnlySelAdapter:BaseDataAdaptor
    {
        public EmployeesOnlySelAdapter(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            try
            {
                return map.Map<List<EmployeeAllReturnView>>(await ((await baseHttpClient.Client()).SelectEmployeesAllAsync()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
