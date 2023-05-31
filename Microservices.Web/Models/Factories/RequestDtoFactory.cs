using static Microservices.Web.Utility.StaticDetails;

namespace Microservices.Web.Models.Factories
{
    public static class RequestDtoFactory
    {
        public static RequestDto CreateRequestDto(ApiType apiType, string url, object? body = null, string? token = "")
        {
            return new RequestDto()
            {
                ApiType = apiType,
                Url = url,
                Body = body,
                Token = token
            };
        }
    }
}
