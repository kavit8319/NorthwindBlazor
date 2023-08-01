using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class UserReturnProfiler:Profile
    {
        public UserReturnProfiler()
        {
            CreateMap<UserReturnView, UserReturn>().ForMember(el=>el.UserID,t=>t.MapFrom(f=>f.Id)).ReverseMap();
            CreateMap<UserReturn, UserReturnView>().ForMember(el => el.PasswordConfirm, t => t.MapFrom(a=>"123456789"))
                .ForMember(el=>el.Password,t=>t.MapFrom(a=>"123456789")).
                ForMember(el=>el.Id,t=>t.MapFrom(el=>el.UserID)).ReverseMap();
            CreateMap<UserReturnResult,UserReturnView>().ForMember(el=>el.Id,t=>t.MapFrom(f=>f.Data.UserID)).ReverseMap();
        }
    }
}
