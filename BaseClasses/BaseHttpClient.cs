using Microsoft.JSInterop;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;

namespace Northwind.Interface.Server.BaseClasses
{
    public class BaseHttpClient
    {
        public IServiceCollectionProvider ServiceCollectionProv { get; set; }
        public ILocalStorageService LocalStoreService { get; set; }
        public BaseHttpClient(IServiceCollectionProvider serviceCollectionProv, ILocalStorageService localStoreService)
        {
            this.ServiceCollectionProv = serviceCollectionProv;
            LocalStoreService = localStoreService;
        }

        public async Task<Client> Client()
        {
            try
            {
                var servBuild = ServiceCollectionProv.ServiceCollection.BuildServiceProvider();
                var localFactory = servBuild?.GetRequiredService<IHttpClientFactory>();
                var ser = servBuild?.GetRequiredService<Client>();
                if (ser != null && localFactory != null)
                    ser.clientFactory = localFactory;
                if (ser != null && LocalStoreService != null && !string.IsNullOrEmpty((await LocalStoreService.GetItemAsync(Constans.AccessToken))))
                {
                    var res = await LocalStoreService.GetItemAsync(Constans.AccessToken);
                    ser.Token = res;
                    return ser;
                }
                return ser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
