using Microsoft.Extensions.Options;

namespace Northwind.Interface.Server.Pages.LoginPages.reCaptcha
{
    public class CaptchaApi
    {
        private IHttpClientFactory HttpClientFactory { get; }

        private IOptionsMonitor<CaptchConfiguration> reCAPTCHAVerificationOptions { get; }

        public CaptchaApi(IHttpClientFactory httpClientFactory, IOptionsMonitor<CaptchConfiguration> reCAPTCHAVerificationOptions)
        {
            this.HttpClientFactory = httpClientFactory;
            this.reCAPTCHAVerificationOptions = reCAPTCHAVerificationOptions;
        }

        public async Task<(bool Success, string[] ErrorCodes)> Post(string reCAPTCHAResponse)
        {
            var url = "https://www.google.com/recaptcha/api/siteverify";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret", this.reCAPTCHAVerificationOptions.CurrentValue.Secret},
                {"response", reCAPTCHAResponse}
            });

            var httpClient = this.HttpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var verificationResponse = await response.Content.ReadFromJsonAsync<CaptchaResponse>();
            if (verificationResponse.Success) return (Success: true, ErrorCodes: new string[0]);

            return (
                Success: false,
                ErrorCodes: verificationResponse.ErrorCodes.Select(err => err.Replace('-', ' ')).ToArray());
        }
    }
}
