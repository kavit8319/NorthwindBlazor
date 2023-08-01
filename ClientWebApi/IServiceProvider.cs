namespace Northwind.Interface.Server.ClientWebApi
{
    public interface IServiceCollectionProvider
    {
        IServiceCollection ServiceCollection { get; }
    }

    public sealed class ServiceCollectionProvider : IServiceCollectionProvider
    {
        public ServiceCollectionProvider(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }
    }
}
