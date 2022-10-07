using BlogApp.WebApi.ViewModels.Users;
using MimeKit;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendAsync(EmailMessageViewModel emailMessage);
    }
}
