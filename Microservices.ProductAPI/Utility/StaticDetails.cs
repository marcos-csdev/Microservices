namespace Microservices.ProductAPI.Utility
{
    public static class StaticDetails
    {
        public static string ProductAPIBase { get; set; } = string.Empty;
        public static string AuthAPIBase { get; set; } = string.Empty;

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
