using System.Security.AccessControl;
using static Microservices.Web.Utility.StaticDetails;

namespace Microservices.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = null!;
        public object Body { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
