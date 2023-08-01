using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class CustomerReturnProfiler:Profile
    {
        public CustomerReturnProfiler()
        {
            CreateMap<CustomerReturn, CustomerReturnView>().ReverseMap();
        }
    }
}
