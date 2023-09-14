

using Microservices.ProductAPI.Models.Dto;

namespace Microservices.ProductAPI.Models.Factories
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
                Result = result!
            };
        }
        public static ResponseDto CreateResponseDto(bool isSuccess, object? result, string message)
        {
            return new ResponseDto()
            {
                DisplayMessage = message,
                IsSuccess = isSuccess,
                Result = result!
            };
        }
    }
}
