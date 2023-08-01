using Newtonsoft.Json;

namespace Northwind.Interface.Server.ClientWebApi
{
    public class ResponsError
    {
        private ApiException api;

      
        public ResponsError(ApiException error)
        {
            api = error;
            errorMessage = null;
        }
        public string type { get; set; } = default;
        public string title { get; set; } = default;

        private string errorMessage=default;
        public string ErrorMesage
        {
            get
            {
                if (api != null && !string.IsNullOrEmpty(api.Response))
                    return errorMessage = JsonConvert.DeserializeObject<ResponsError>(api.Response)!.ErrorMesage;
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }
    }
}
