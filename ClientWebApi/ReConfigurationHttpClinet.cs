using System.Reflection;
using Microsoft.JSInterop;
using Northwind.Interface.Server.Authentification;

namespace Northwind.Interface.Server.ClientWebApi
{
    public class ReConfigurationHttpClinet : IReConfigurationHttpClinet
    {
        IServiceCollectionProvider Config { get; set; }
        private readonly ILocalStorageService LocalStorageService;
        public ReConfigurationHttpClinet(IServiceCollectionProvider serviceCollectionProvider, ILocalStorageService _localStoreService)
        {
            Config = serviceCollectionProvider;
            LocalStorageService = _localStoreService;
        }

        public async Task<Client> Client()
        {
            try
            {
                var servBuild = Config.ServiceCollection.BuildServiceProvider();
                var localFactory = servBuild.GetRequiredService<IHttpClientFactory>();
                var ser = servBuild.GetRequiredService<Client>();
                ser.clientFactory = localFactory;
                if (!string.IsNullOrEmpty((await LocalStorageService.GetItemAsync("token"))))
                {
                    var res = (await LocalStorageService.GetItemAsync("token"));
                    ser.Token = res;
                }

                MethodInfo dynMethod = this.GetType().GetMethod("CreateSerializerSettings",BindingFlags.NonPublic | BindingFlags.Instance);
                dynMethod.Invoke(ser, new object[]{});
                
                return ser;
            }
            catch (Exception)
            {
                throw ;
            }
        }
        public ValueTask<bool> IsAuthenticated()
        {
            return new ValueTask<bool>(true);
            //return LocalStorageService.ContainKeyAsync("token");
        }
    }
}
