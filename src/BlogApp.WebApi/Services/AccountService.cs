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
        private readonly IFileService _fileservice;
        private readonly IAuthManager _authManager;

        public AccountService(IUserRepository userRepository, IFileService fileService,
                              IAuthManager authManager)
        {
            _repositroy = userRepository;
            _fileservice = fileService;
            _authManager = authManager;
        }

        public async Task<string?> LogInAsync(UserLogInViewModel viewModel)
        {
            var user = await _repositroy.GetAsync(o => o.Email == viewModel.Email);

            if (user is not null)
            {
                if (PasswordHasher.Verify(viewModel.Password, user.Salt, user.PasswordHash))
                    return _authManager.GenerateToken(user);

                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "password is wrong");
            }
            throw new StatusCodeException(HttpStatusCode.NotFound, message: "email is wrong");
        }

        public async Task<bool> RegistrAsync(UserCreateViewModel viewModel)
        {
            var userk = await _repositroy.GetAsync(o => o.Email == viewModel.Email);

            if (userk is null)
            {
                var user = (User)viewModel;

                if (user.ImagePath is not null)
                    user.ImagePath = await _fileservice.SaveImageAsync(viewModel.Image);

                var hashResult = PasswordHasher.Hash(viewModel.Password);

                user.Salt = hashResult.Salt;

                user.PasswordHash = hashResult.Hash;

                var result = await _repositroy.CreateAsync(user);

                await _repositroy.SaveAsync();

                var email = new SendCodeToEmailViewModel()
                {
                    Email = viewModel.Email,
                };
            }

            throw new StatusCodeException(HttpStatusCode.BadRequest, message: "user already exist!");
        }

        public async Task<bool> VerifyPasswordAsync(ForgotPassword password)
        {
            var user = await _repositroy.GetAsync(p => p.Email == password.Email);

            if(user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "user not found!");

            var changedPassword = PasswordHasher.ChangePassword(password.Password, user.Salt);

            user.PasswordHash = changedPassword;

            await _repositroy.UpdateAsync(user);
            await _repositroy.SaveAsync();

            return true;
        }
    }
}