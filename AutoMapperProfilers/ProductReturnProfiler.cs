using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class ProductReturnProfiler:Profile
    {
        public ProductReturnProfiler()
        {
            CreateMap<ProductReturn, ProductReturnView>().ReverseMap();
        }
    }
}
