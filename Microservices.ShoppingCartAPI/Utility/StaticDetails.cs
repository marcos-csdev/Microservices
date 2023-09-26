namespace Microservices.ShoppingCartAPI.Utility
{
    public static class StaticDetails
    {

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";

        public static string CouponAPIUrl { get; set; } = string.Empty;
        public static string ProductAPIUrl { get; set; } = string.Empty;

        public const string AppType = "application/json";
        public const string CartAPIName = "ShoppingCartAPIClient";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
