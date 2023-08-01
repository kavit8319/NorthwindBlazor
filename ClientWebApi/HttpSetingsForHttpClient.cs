using Newtonsoft.Json;
namespace Northwind.Interface.Server.ClientWebApi
{
    public partial class HttpSetingsForHttpClient
    {
        [Microsoft.AspNetCore.Components.Inject]
        public IHttpClientFactory clientFactory { get; set; }

        //private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public string Token { get; set; }
        public HttpSetingsForHttpClient()
        {
        }

        protected Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            HttpClient client;
            if (clientFactory != null)
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    client = clientFactory.CreateClient("UserServiceAuth");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                }
                else
                    client = clientFactory.CreateClient("UserServiceSimple");
                return Task.FromResult(client);
            }
            return Task.FromResult(new HttpClient());
        }

        protected JsonSerializerSettings GenerateUpdateJsonSerializeSettings(
            JsonSerializerSettings settings)
        {
            settings.DateFormatString = "dd.MM.yyyy";
            return settings;
        }
    }
}
