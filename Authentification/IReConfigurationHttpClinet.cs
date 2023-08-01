namespace Northwind.Interface.Server.Authentification
{
    public interface IReConfigurationHttpClinet
    {
        public ValueTask<bool> IsAuthenticated();
        public Task<ClientWebApi.Client> Client();
    }
}
