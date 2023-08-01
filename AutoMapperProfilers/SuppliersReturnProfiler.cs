using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class SuppliersReturnProfiler : Profile
    {
        public SuppliersReturnProfiler()
        {
            CreateMap<SuppliersReturnView, SupplierReturn>();
            CreateMap<SupplierReturn, SuppliersReturnView>().ForMember(el=>el.IdStr,el=>el.MapFrom(el=>el.Id.ToString()));
        }
    }
}
