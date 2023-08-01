using AutoMapper;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;

namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public class CategoryReturnProfile:Profile
    {
        public CategoryReturnProfile()
        {
            CreateMap<CategoryView, CategoryReturnView>().ForMember(el=>el.IdStr,el=>el.MapFrom(el=>el.Id.ToString())).ReverseMap();
          
        }
    }
}
