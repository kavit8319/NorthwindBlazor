namespace Northwind.Interface.Server.AutoMapperProfilers
{
    public static class AddAutoMapperProfilers
    {
        public static void AddAutoMapProffilers(this IServiceCollection service)
        {
            var array = new[] {
                typeof(UserReturnProfiler),
                typeof(SuppliersReturnProfiler),
                typeof(CategoryReturnProfile),
                typeof(ProductReturnProfiler),
                typeof(EmployeesReturnProfiler),
                typeof(CustomerReturnProfiler),
                typeof(ShipperReturnProfile),
                typeof(OrderReturnProfiler),
                typeof(OrderDetailReturnProfile),
                typeof(EmployeeAllProfile)
            };
            service.AddAutoMapper(array);
        }
    }
}
