using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class OrderReturnProfiler:Profile
    {
        public OrderReturnProfiler()
        {
            CreateMap<OrderReturn, OrderReturnView>().ReverseMap();
        }
    }
}
