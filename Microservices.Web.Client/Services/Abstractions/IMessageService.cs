using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IMessageService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }

        Task<ResponseDto?> SendAsync(RequestDto apiRequest, bool withBearer = true, string clientName = "MicroServicesClient");
    }
}
