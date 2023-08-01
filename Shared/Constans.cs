namespace Northwind.Interface.Server.Shared
{
    public class Constans
    {
        public const string CountryCode = "code";
        public const string CountryFilter = "filter";
        public const string CountryCity = "city";
        public const string CountryCountry = "country";
        public const string CountryPostCode = "postcode";
        public const string Category="category";
        public const string Employee = "employee";
        public const string Comfirm = "comfirm";
        public const string ProductId = "productId";
        public const string ProductIds = "productIds";
        public const string OrderId = "OrderId";

        public const string AccessToken = "token";
        public const string RefreshToken = "refreshToken";
        public static TimeSpan MemoryCashMinute => TimeSpan.FromMinutes(5);
    }
}
