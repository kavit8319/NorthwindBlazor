using static Northwind.Interface.Server.Pages.TestPages.TreeGreedPaging;

namespace Northwind.Interface.Server.Adaptors
{
    public static class AddScopeDataAdaptors
    {
        public static void AddCustomSyncfusionAdaptors(this IServiceCollection service)
        {
            service.AddScoped<UserAdmAdaptor>();
            service.AddScoped<UserRoleAdmAdapter>();
            service.AddScoped<CountrysAdaptersddl>();
            service.AddScoped<CityAdapterddl>();
            service.AddScoped<PostCodeAdapterddl>();
            service.AddScoped<SuppliersAdaptor>();
            service.AddScoped<CategoryAdaptor>();
            service.AddScoped<ProductsAdaprtor>();
            service.AddScoped<EmployeesAdaptor>();
            service.AddScoped<TitleOfCourtesyAdaptor>();
            service.AddScoped<TitlesAdaprter>();
            service.AddScoped<ReportToAdaptor>();
            service.AddScoped<CustomersAdapter>();
            service.AddScoped<CustomerTitleAdaptor>();
            service.AddScoped<ShipperAdaptor>();
            service.AddScoped<OrdersAdaptor>();
            service.AddScoped<CustomerOnlySelAdapter>();
            service.AddScoped<EmployeesOnlySelAdapter>();
            service.AddScoped<OrderDetailAdapter>();
        }
    }
}
