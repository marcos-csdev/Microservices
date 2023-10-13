using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Microservices.ShoppingCartAPI.Utility
{
    /// <summary>
    /// DelegatingHandlers are useful to add processing to the regular .NET CORE request
    /// </summary>
    public class AuthenticationHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_contextAccessor.HttpContext != null)
            {
                var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
