namespace Microservices.ShoppingCartAPI.Utility
{
    public static class StaticDetails
    {

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";

        public const string CouponAPIURL = "/api/coupon/GetByCode";
        public const string ProductAPIURL = "/api/product";

        public const string AppType = "application/json";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
