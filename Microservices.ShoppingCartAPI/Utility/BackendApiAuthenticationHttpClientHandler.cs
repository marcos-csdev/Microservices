using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Microservices.ShoppingCartAPI.Utility
{
    /// <summary>
    /// DelegatingHandlers are useful to add processing to the regular .NET CORE request
    /// </summary>
    public class BackendApiAuthenticationHttpClientHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_contextAccessor.HttpContext != null)
            {
                var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer");
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
