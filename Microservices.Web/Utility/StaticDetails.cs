﻿namespace Microservices.Web.Utility
{
    public static class StaticDetails
    {
        public static string CouponAPIBase { get; set; } = string.Empty;
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
