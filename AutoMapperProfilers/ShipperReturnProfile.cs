using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class ShipperReturnProfile:Profile
    {
        public ShipperReturnProfile()
        {
            CreateMap<ShipperReturn, ShipperReturnView>().ReverseMap();
        }
    }
}
