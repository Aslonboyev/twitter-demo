using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IAccountService
    {
        Task RegistrAsync(UserCreateViewModel model);

        Task<string?> LogInAsync(UserLogInViewModel model);


        Task VerifyPasswordAsync(UserResetPasswordViewModel password);
    }
}
