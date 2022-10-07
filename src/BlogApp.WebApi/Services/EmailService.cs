using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.ViewModels.Users;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BlogApp.WebApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration.GetSection("Email");
        }

        public async Task SendAsync(EmailMessageViewModel emailMessage)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config["Email"]));
            email.To.Add(MailboxAddress.Parse(emailMessage.To));
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text =emailMessage.Body.ToString()};

            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Host"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Email"], _config["Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}