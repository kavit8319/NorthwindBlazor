using AutoMapper;
using Northwind.Interface.Server.BaseClasses;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Adaptors
{
    public class TitleOfCourtesyAdaptor:BaseDataAdaptor
    {
        public TitleOfCourtesyAdaptor(BaseHttpClient http, IMapper mapper) : base(http, mapper)
        {
        }


        public override object Read(DataManagerRequest dataManagerRequest, string key = null)
        {
            return new List<string>() {"Mr.","Ms.","Dr.","Mrs."};
        }
    }
}
