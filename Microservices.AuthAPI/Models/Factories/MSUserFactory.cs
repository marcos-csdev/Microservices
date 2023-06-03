namespace Microservices.AuthAPI.Models.Factories
{
    public static class MSUserFactory
    {
        public static MSUser Create(string userName, string email, string normalizedEmail, string name, string phoneNumber)
        {
            return new MSUser
            {
                UserName = userName,
                Email = email,
                NormalizedEmail = normalizedEmail,
                Name = name,
                PhoneNumber = phoneNumber
            };
        }
    }
}
