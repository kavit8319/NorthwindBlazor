using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class EmployeeAllProfile:Profile
    {
        public EmployeeAllProfile()
        {
            CreateMap<EmployeeAllReturn, EmployeeAllReturnView>()
                .ForMember(el => el.FullName, el => el.MapFrom(el => el.FullName))
                .ForMember(el => el.Id, el => el.MapFrom(el => el.Id))
                .ForMember(el => el.Title, el => el.MapFrom(el => el.JobTitle));

        }
    }
}
