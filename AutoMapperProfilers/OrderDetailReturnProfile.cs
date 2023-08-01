using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class OrderDetailReturnProfile:Profile
    {
        public OrderDetailReturnProfile()
        {
            CreateMap<OrderDetailReturn, OrderDetailReturnView>().ReverseMap();
        }
    }
}
