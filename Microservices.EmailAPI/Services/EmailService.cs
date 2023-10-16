using Microservices.EmailAPI.Models.Dto;
using Microservices.EmailAPI.Models.Factories;
using Microservices.EmailAPI.Repositories;
using System.Text;

namespace Microservices.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task CreateEmail(CartDto cartDto)
        {
            var message = ComposeEmail(cartDto);

            await SaveEmail(message.ToString(), cartDto.CartHeader!.Email!);
        }

        private string ComposeEmail(CartDto cartDto)
        {
            var message = new StringBuilder();
            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader!.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails!)
            {
                message.Append("<li>");
                message.Append(item.ProductDto!.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            return message.ToString();  
        }
        public async Task RegisterUserEmail(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await SaveEmail(message, email);

        }

        private async Task<int> SaveEmail(string message, string email)
        {
            var emailLogger = EmailLoggerFactory.Create(message, email);

            return await _emailRepository.SaveEmail(emailLogger);
        }
    }
}
