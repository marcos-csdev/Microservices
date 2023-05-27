namespace Microservices.CouponAPI.Models.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; } = null!;
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; } = null!;
    }
}
