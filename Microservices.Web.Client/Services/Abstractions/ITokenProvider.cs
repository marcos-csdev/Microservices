namespace Microservices.Web.Client.Services.Abstractions
{
    public interface ITokenProvider
    {
        void SetToken(string token);

        string? GetToken();

        void ClearToken();
    }
}
