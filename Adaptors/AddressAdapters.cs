using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using Northwind.Interface.Server.BaseClasses;

namespace Northwind.Interface.Server.Adaptors
{

    public class CountrysAdaptersddl : BaseDataAdaptor
    {
        private readonly IMemoryCache cash;
        public CountrysAdaptersddl(BaseHttpClient http, IMemoryCache mc) : base(http)
        {
            cash = mc;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            IEnumerable<Country> listCountr = new List<Country>();

            if (dm.Params == null)
                cash?.Remove(Constans.CountryCountry);
            listCountr = cash?.Get<IEnumerable<Country>>(Constans.CountryCountry);
            if (listCountr == null)
            {
                listCountr = (await (await baseHttpClient.Client()).GetAllCountryAsync());
                cash?.Set(Constans.CountryCountry, listCountr, Constans.MemoryCashMinute);
            }

            if (dm.Params != null && dm.Params.TryGetValue(Constans.CountryFilter, out var obj))
            {
                if (obj != null)
                {
                    var keyPres = (string)obj;
                    listCountr = listCountr.Where(el => el.Name.ToLower().Contains(value: keyPres)).ToList();
                }
                }
            else
                cash?.Remove(Constans.CountryCountry);

            return dm.RequiresCounts ? new DataResult() { Result = listCountr, Count = listCountr.Count() } : listCountr.ToList();
        }
    }

    public class CityAdapterddl : BaseDataAdaptor
    {
        private readonly IMemoryCache cash;
        private string code;
        public CityAdapterddl(BaseHttpClient http,IMemoryCache mc) : base(http) => cash = mc;

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            IEnumerable<string> result = new List<string>();
            if (dm != null && dm.Params != null && dm.Params.TryGetValue(Constans.CountryCode, out var obj))
            {
                if (obj != null) { code = (string)obj; }
                var codelocal = cash?.Get(Constans.CountryCode);
                if (code != null && !code.Equals(codelocal))
                {
                    cash?.Remove(obj);
                    cash?.Remove(Constans.CountryCode);
                    cash?.Remove(Constans.CountryPostCode);
                    cash?.Set(Constans.CountryCode, code);
                }

                result = cash?.Get<IEnumerable<string>>(obj);
                if (result == null)
                {
                    result = (await (await baseHttpClient.Client()).GetAllCityAsync(code));
                    cash?.Set(obj, result, Constans.MemoryCashMinute);
                    cash?.Set(Constans.CountryCode, code, Constans.MemoryCashMinute);
                }
                return dm.RequiresCounts ? new DataResult() { Result = result, Count = result.Count() } : result.ToList();
            }
            if (dm != null && dm.Params != null && dm.Params.TryGetValue(Constans.CountryFilter, out obj))
            {
                var countryCode = cash?.Get(Constans.CountryCode);
                result = cash?.Get<IEnumerable<string>>(countryCode);
                var filterKey = (string)obj;
                if (result != null)
                    result = result.Where(el => el.ToLower().Contains(filterKey)).ToList();
                return dm.RequiresCounts ? new DataResult() { Result = result, Count = result.Count() } : result.ToList();
            }
            ClearUtil.Clear(dm, cash);
            cash?.Remove(Constans.CountryCity);
            return new object();
        }
    }

    public class PostCodeAdapterddl : BaseDataAdaptor
    {
        private readonly IMemoryCache cash;
        private string code;
        public PostCodeAdapterddl(BaseHttpClient http, IMemoryCache mc) : base(http)
        {
            cash = mc;
        }
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            if (dm.Params != null && dm.Params.ContainsKey(Constans.CountryCode))
            {
                IEnumerable<string> result = new List<string>();
                dm.Params.TryGetValue(Constans.CountryCode, out var obj);
                if (obj != null) { code = (string)obj; }
                var codelocal = cash.Get(Constans.CountryCode);
                if (code != null && !code.Equals(codelocal))
                {
                    cash.Remove(Constans.CountryPostCode);
                    cash.Remove(Constans.CountryCode);
                    cash.Set(Constans.CountryCode, code, Constans.MemoryCashMinute);
                }
                result = cash.Get<IEnumerable<string>>(Constans.CountryPostCode);
                if (result == null&&code!=null)
                {
                    result = (await (await baseHttpClient.Client()).GetAllPostCodeAsync(code));
                    cash.Set(Constans.CountryPostCode, result, Constans.MemoryCashMinute);
                    cash.Set(Constans.CountryCode, code, Constans.MemoryCashMinute);
                }

                if (dm.Params.TryGetValue(Constans.CountryFilter, out obj))
                {

                    var filterKey = (string)obj;
                    if (filterKey.Length == 2)
                        result = result.Where(el => el.ToLower().Contains(filterKey)).ToList();
                    else if (filterKey != null)
                        result = result.Where(el => el.ToLower().Contains(value: filterKey)).Take(20).ToList();
                }
                else
                    result = result.Take(10);
               ClearUtil.Clear(dm,cash);
                return dm.RequiresCounts ? new DataResult() { Result = result, Count = result.Count() } : result.ToList();
            }
            cash?.Remove(Constans.CountryPostCode);
            return new object();
        }
    }

    public class ClearUtil
    {
        public static void Clear(DataManagerRequest dm, IMemoryCache cash)
        {
            if (dm != null && dm.Params != null && dm.Params.TryGetValue("clear", out var obj))
            {
                if (obj != null)
                {
                    var country = cash?.Get(Constans.CountryCode);
                    cash.Remove(country);
                    cash.Remove(Constans.CountryCode);
                    cash.Remove(Constans.CountryPostCode);
                }
            }
        }
    }
}
