namespace Microservices.Web.Client.Utility
{
    public static class StaticDetails
    {
        public static string CouponAPIUrl { get; set; } = string.Empty;
        public static string ProductAPIUrl { get; set; } = string.Empty;
        public static string AuthAPIUrl { get; set; } = string.Empty;

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
