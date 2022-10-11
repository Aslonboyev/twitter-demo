using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _repository;

        public VerifyEmailService(IMemoryCache cache, IEmailService email,
                                  IUserRepository repository)
        {
            _cache = cache;
            _emailService = email;
            _repository = repository;
        }

        public async Task SendCodeAsync(SendCodeToEmailViewModel email)
        {
            int code = new Random().Next(1000, 9999);

            _cache.Set(email.Email, code, TimeSpan.FromMinutes(2));

            var message = new EmailMessageViewModel()
            {
                To = email.Email,
                Subject = "Verification code",
                Body = code,
            };

            await _emailService.SendAsync(message);
        }

        public async Task<bool> VerifyEmail(EmailVerifyViewModel emailVerify)
        {
            var entity = await _repository.GetAsync(user => user.Email == emailVerify.Email && user.ItemState == ItemState.Active);

            if (entity == null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found!");

            if (_cache.TryGetValue(emailVerify.Email, out int exceptedCode))
            {
                if (exceptedCode != emailVerify.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong!");

                entity.IsEmailConfirmed = true;

                await _repository.UpdateAsync(entity);

                await _repository.SaveAsync();

                return true;
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }

        public Task<bool> VerifyPasswordAsync(UserResetPasswordViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
