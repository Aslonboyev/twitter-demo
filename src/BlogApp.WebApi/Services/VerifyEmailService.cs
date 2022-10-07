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

        public async Task SendCodeAsync(SendToEmail email)
        {
            int code = new Random().Next(1000, 9999);

            _cache.Set(email.Email, code, TimeSpan.FromMinutes(10));

            var message = new EmailMessage()
            {
                To = email.Email,
                Subject = "Verification code",
                Body = code,
            };

            await _emailService.SendAsync(message);
        }

        public async Task<bool> VerifyEmail(EmailVerify emailVerify)
        {
            var entity = await _repository.GetAsync(user => user.Email == emailVerify.Email);

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
    }
}
