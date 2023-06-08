using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }

        Task<T?> SendAsync<T>(RequestDto apiRequest);
    }
}
