using Microservices.Web.Utility;
using static Microservices.Web.Utility.StaticDetails;

namespace Microservices.Web.Models.Factories
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
