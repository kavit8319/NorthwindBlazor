using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class EmployeesReturnProfiler : Profile
    {
        public EmployeesReturnProfiler()
        {
            CreateMap<EmployeeReturn, EmployeeReturnView>().ForMember(el=>el.isParent,el=>el.MapFrom(el=>el.IsParent==1?true:(bool?)null)).ReverseMap();
            
        }
    }
}
