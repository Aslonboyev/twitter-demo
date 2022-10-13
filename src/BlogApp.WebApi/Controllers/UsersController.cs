using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<IActionResult> GetAllAsync([FromQuery]PaginationParams @params)
        {
            return Ok(await _service.GetAllAsync(@params, null));
        }
        
        [HttpPatch("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateIamgeAsync(long id, [FromForm] UserImageUpdateViewModel userCreateViewModel)
        {
            return Ok(await _service.ImageUpdate(id, userCreateViewModel));
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody]UserCreateViewModel userCreateViewModel)
        {
            return Ok(await _service.UpdateAsync(id, userCreateViewModel));
        }

        [HttpGet("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await _service.GetAsync(p => p.Id == id));
        }

        [HttpDelete("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            return Ok(await _service.DeleteAsync(p => p.Id == id));
        }

    }
}