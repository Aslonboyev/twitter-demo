using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public VerifyEmailService(IMemoryCache cache, IEmailService email, AppDbContext appDbContext)
        {
            _cache = cache;
            _emailService = email;
            _context = appDbContext;
        }

        public async Task SendCodeAsync(SendCodeToEmailViewModel email)
        {
            int code = new Random().Next(1000, 9999);

            _cache.Set(email.Email, code, TimeSpan.FromMinutes(3));

            var message = new EmailMessageViewModel()
            {
                To = email.Email,
                Subject = "Verification code",
                Body = code,
            };

            await _emailService.SendAsync(message);
        }

        public async Task VerifyEmail(EmailVerifyViewModel emailVerify)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Email == emailVerify.Email && user.ItemState == ItemState.Active);

            if (entity == null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found!");

            if (_cache.TryGetValue(emailVerify.Email, out int exceptedCode))
            {
                if (exceptedCode != emailVerify.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong!");

                entity.IsEmailConfirmed = true;

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }

        public async Task VerifyPasswordAsync(UserResetPasswordViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == model.Email);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "user not found");

            if (_cache.TryGetValue(model.Email, out int code))
            {
                if (code != model.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong");
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }
    }
}
