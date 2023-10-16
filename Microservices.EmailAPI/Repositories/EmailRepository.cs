using Microservices.EmailAPI.Data;
using Microservices.EmailAPI.Models;

namespace Microservices.EmailAPI.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MsDbContext _context;

        public EmailRepository(MsDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveEmail(EmailLogger emailLogger)
        {
            await _context.EmailLoggers.AddAsync(emailLogger);

            return await _context.SaveChangesAsync();
        }
    }
}
