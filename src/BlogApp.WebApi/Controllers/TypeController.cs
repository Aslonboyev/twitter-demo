using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.PostTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/types")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IPostTypeService _service;

        public TypeController(IPostTypeService service)
        {
            _service = service;
        }

        [HttpGet, Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        {
            return Ok(await _service.GetAllAsync(@params));
        }

        [HttpGet("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await _service.GetAsync(p => p.Id == id));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync(PostTypeCreateViewModel model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpDelete("id"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.DeleteAsync(p => p.Id == id);
            return Ok();
        }

        [HttpPut("id"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(long id, PostTypeCreateViewModel model)
        {
            return Ok(await _service.UpdateAsync(id, model));
        }
    }
}
