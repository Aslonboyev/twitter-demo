using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("registr")]
        public async Task<IActionResult> Registr([FromForm]UserCreateViewModel userCreateViewModel)
            => Ok(await _acountService.RegistrAsync(userCreateViewModel));

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromForm]UserLogInViewModel logInViewModel)
            => Ok(new {Token = await _acountService.LogInAsync(logInViewModel)});

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromForm] EmailVerify email)
            => Ok(await _emailService.VerifyEmail(email));

        [HttpPost("send-code-to-email")]
        public async Task<IActionResult> SendToEmail([FromForm] SendToEmail email)
        {
            await _emailService.SendCodeAsync(email);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ForgotPassword([FromForm]ForgotPassword forgotPassword)
        {
            return Ok();
        }
    }
}
