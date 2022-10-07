using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> RegistrAsync(UserCreateViewModel model);

        Task<string?> LogInAsync(UserLogInViewModel model);

        Task<bool> VerifyPasswordAsync(UserResetPasswordViewModel password);
    }
}
