using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }

        Task<ResponseDto?> SendAsync(RequestDto apiRequest);
    }
}
