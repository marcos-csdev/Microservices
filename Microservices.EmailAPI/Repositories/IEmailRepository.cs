using Microservices.EmailAPI.Models;

namespace Microservices.EmailAPI.Repositories
{
    public interface IEmailRepository
    {
        Task<int> SaveEmail(EmailLogger emailLogger);
    }
}