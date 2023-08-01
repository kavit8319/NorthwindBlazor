using System.Net;

namespace Northwind.Interface.Server.Authentification
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //token = await RefreshTokenAsync();
            //request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);
            return await base.SendAsync(request, cancellationToken);

        }
    }
}
