using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IVerifyEmailService
    {
        Task SendCodeAsync(SendCodeToEmailViewModel email);

        Task<bool> VerifyEmail(EmailVerifyViewModel emailVerify);

        Task<bool> VerifyPasswordAsync(UserResetPasswordViewModel model);
    }
}
