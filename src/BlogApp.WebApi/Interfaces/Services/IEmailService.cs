using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendAsync(EmailMessageViewModel emailMessage);
    }
}
