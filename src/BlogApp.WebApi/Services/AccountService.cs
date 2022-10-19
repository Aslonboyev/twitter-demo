using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Security;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IAuthManager _authManager;

        public AccountService(AppDbContext appDbContext, IAuthManager authManager)
        {
            _context = appDbContext;
            _authManager = authManager;
        }

        public async Task<string?> LogInAsync(UserLogInViewModel viewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(o => o.Email == viewModel.Email && o.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "email is wrong");

            if (user.IsEmailConfirmed is false)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "email did not verified!");

            if (PasswordHasher.Verify(viewModel.Password, user.Salt, user.PasswordHash))
                return _authManager.GenerateToken(user);

            throw new StatusCodeException(HttpStatusCode.BadRequest, message: "password is wrong");
        }

        public async Task RegistrAsync(UserCreateViewModel viewModel)
        {
            var email = await _context.Users.FirstOrDefaultAsync(o => o.ItemState == ItemState.Active && o.Email == viewModel.Email);

            if (email is not null)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Email already exist");

            var username = await _context.Users.FirstOrDefaultAsync(p => p.UserName == viewModel.UserName && p.ItemState == ItemState.Active);

            if (username is not null)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Username already exist");

            var user = (User)viewModel;

            var hashResult = PasswordHasher.Hash(viewModel.Password);

            user.Salt = hashResult.Salt;

            user.ItemState = ItemState.Active;

            user.PasswordHash = hashResult.Hash;

            user.CreatedAt = DateTime.UtcNow;

            var result = _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task VerifyPasswordAsync(UserResetPasswordViewModel password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == password.Email && p.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "user not found!");

            if (user.IsEmailConfirmed is false)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "email did not verified!");

            var changedPassword = PasswordHasher.ChangePassword(password.Password, user.Salt);

            user.PasswordHash = changedPassword;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}