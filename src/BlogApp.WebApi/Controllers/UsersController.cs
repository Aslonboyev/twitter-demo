using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        {
            return Ok(await _service.GetAllAsync(@params));
        }

        [HttpPatch(), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAsync([FromForm] UserPatchViewModel userCreateViewModel)
        {
            return Ok(await _service.UpdateAsync(userCreateViewModel));
        }

        [HttpGet("userInfo"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetInfoAsync()
        {
            return Ok(await _service.GetInfoAsync());
        }

        [HttpGet("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAsync(long id)
        {
            
            return Ok(await _service.GetAsync(p => p.Id == id));
        }

        [HttpDelete(), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAsync()
        {
            await _service.DeleteAsync();
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.DeleteAsync(p => p.Id == id);
            return Ok();
        }
    }
}