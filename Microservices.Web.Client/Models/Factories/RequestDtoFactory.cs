using static Microservices.Web.Client.Utility.StaticDetails;

namespace Microservices.Web.Client.Models.Factories
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
