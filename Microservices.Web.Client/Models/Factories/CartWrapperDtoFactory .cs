namespace Microservices.Web.Client.Models.Factories
{
    public static class CartWrapperDtoFactory
    {
        public static CartWrapperDto Create(CartHeaderDto cartHeader, List<CartDetailsDto> cartDetails, List<CouponDto> allCoupons)
        {
            var cartDto =  new CartDto
            {
                CartHeader = cartHeader,
                CartDetails = cartDetails
            };

            var cartwrapper = new CartWrapperDto
            {
                CartDto = cartDto,
                CouponsList = allCoupons
            };

            return cartwrapper;
        }
        public static CartWrapperDto Create(CartDto cartDto, List<CouponDto> allCoupons)
        {
            var cartwrapper = new CartWrapperDto
            {
                CartDto = cartDto,
                CouponsList = allCoupons
            };

            return cartwrapper;
        }
    }
}
