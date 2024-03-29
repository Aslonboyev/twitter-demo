﻿using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _acountService;
        private readonly IVerifyEmailService _emailService;

        public AccountsController(IAccountService accountService, IVerifyEmailService emailService)
        {
            _acountService = accountService;
            _emailService = emailService;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Registr([FromBody] UserCreateViewModel userCreateViewModel)
        {
            await _acountService.RegistrAsync(userCreateViewModel);
            return Ok();
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] UserLogInViewModel logInViewModel)
            => Ok((new { Token = (await _acountService.LogInAsync(logInViewModel)) }));

        [HttpPost("verifyEmail"), AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerifyViewModel email)
        {
            await _emailService.VerifyEmail(email);
            return Ok();
        }

        [HttpPost("sendCodeToEmail"), AllowAnonymous]
        public async Task<IActionResult> SendToEmail([FromBody] SendCodeToEmailViewModel email)
        {
            await _emailService.SendCodeAsync(email);
            return Ok();
        }

        [HttpPost("resetPassword"), AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] UserResetPasswordViewModel forgotPassword)
        {
            await _acountService.VerifyPasswordAsync(forgotPassword);
            return Ok();
        }
    }
}
