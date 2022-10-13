using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Security;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repositroy;
        private readonly IAuthManager _authManager;

        public AccountService(IUserRepository userRepository, IAuthManager authManager)
        {
            _repositroy = userRepository;
            _authManager = authManager;
        }

        public async Task<(string?, long)> LogInAsync(UserLogInViewModel viewModel)
        {
            var user = await _repositroy.GetAsync(o => o.Email == viewModel.Email && o.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "email is wrong");

            if (user.IsEmailConfirmed is false)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "email did not verified!");

            if (PasswordHasher.Verify(viewModel.Password, user.Salt, user.PasswordHash))
                return (_authManager.GenerateToken(user), user.Id);

            throw new StatusCodeException(HttpStatusCode.BadRequest, message: "password is wrong");
        }

        public async Task<bool> RegistrAsync(UserCreateViewModel viewModel)
        {
            var userk = await _repositroy.GetAsync(o => o.Email == viewModel.Email || o.UserName == viewModel.UserName && o.ItemState == ItemState.Active);

            if (userk is null)
            {
                var user = (User)viewModel;
                
                var hashResult = PasswordHasher.Hash(viewModel.Password);

                user.Salt = hashResult.Salt;

                user.PasswordHash = hashResult.Hash;

                var result = await _repositroy.CreateAsync(user);

                await _repositroy.SaveAsync();

                throw new StatusCodeException(HttpStatusCode.OK, message: "true");
            }

            throw new StatusCodeException(HttpStatusCode.OK, message: "false");
        }

        public async Task<bool> VerifyPasswordAsync(UserResetPasswordViewModel password)
        {
            var user = await _repositroy.GetAsync(p => p.Email == password.Email && p.ItemState == ItemState.Active);

            if(user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "user not found!");

            if (user.IsEmailConfirmed is false)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "email did not verified!");

            var changedPassword = PasswordHasher.ChangePassword(password.Password, user.Salt);

            user.PasswordHash = changedPassword;

            await _repositroy.UpdateAsync(user);
            await _repositroy.SaveAsync();

            throw new StatusCodeException(HttpStatusCode.OK, message: "true");
        }
    }
}