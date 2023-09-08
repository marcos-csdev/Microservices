namespace Microservices.Web.Client.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; } = null!;
        public string DisplayMessage { get; set; } = "";
        public string Role { get; set; } = "";
        public List<string> ErrorMessages { get; set; } = null!;

        public ResponseDto()
        {
                
        }
        public ResponseDto(bool isSuccess, object result, string displayMessage)
        {
            IsSuccess = isSuccess;
            Result = result;
            DisplayMessage = displayMessage;
        }
    }
}
