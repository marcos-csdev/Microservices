using Microservices.Web.Models;

namespace Microservices.Web.Services.Abstractions
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }

        Task<T?> SendAsync<T>(RequestDto apiRequest);
    }
}
