using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IVerifyEmailService
    {
        Task SendCodeAsync(SendToEmail email);

        Task<bool> VerifyEmail(EmailVerify emailVerify);
    }
}
