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
            return Ok(await _service.GetAllAsync(@params, p => p.ItemState == Enums.ItemState.Active));
        }

        [HttpPatch(), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAsync([FromForm] UserPatchViewModel userCreateViewModel)
        {
            await _service.UpdateAsync(userCreateViewModel);
            return Ok();
        }

        [HttpGet("user-info"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetInfoAsync()
        {
            await _service.GetInfoAsync();
            return Ok();
        }

        [HttpGet("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAsync(long id)
        {
            await _service.GetAsync(p => p.Id == id && p.ItemState == Enums.ItemState.Active);
            return Ok();
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
            await _service.DeleteAsync(p => p.Id == id && p.ItemState == Enums.ItemState.Active);
            return Ok();
        }
    }
}