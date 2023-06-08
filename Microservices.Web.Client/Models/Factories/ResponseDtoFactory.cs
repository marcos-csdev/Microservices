using Microservices.Web.Client.Utility;
using static Microservices.Web.Client.Utility.StaticDetails;

namespace Microservices.Web.Client.Models.Factories
{
    public static class ResponseDtoFactory
    {
        public static ResponseDto CreateResponseDto(string message, List<string> errorMessages, bool isSuccess, object? result = null)
        {
            return new ResponseDto()
            {
                DisplayMessage = message,
                ErrorMessages = errorMessages,
                IsSuccess = isSuccess,
                Result = result
            };
        }
    }
}
