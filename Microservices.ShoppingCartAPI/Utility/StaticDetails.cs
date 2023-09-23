namespace Microservices.ShoppingCartAPI.Utility
{
    public static class StaticDetails
    {

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";

        public const string CouponAPIURL = "https://localhost:7001/api/coupon/GetByCode";


        public const string ProductAPIFullUrl = "https://localhost:7000/api/products";

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
