using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.SaveMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveMessageController : ControllerBase
    {
        private readonly ISaveMessageService _service;

        public SaveMessageController(ISaveMessageService messageService)
        {
            _service = messageService;
        }
        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateAsync(SaveMessageCreateViewModel model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpGet("user_id"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllAsync(long user_id, [FromQuery] PaginationParams @params)
        {
            return Ok(await _service.GetAllAsync(user_id, @params));
        }

        [HttpDelete("{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            return Ok(await _service.DeleteAsync(p => p.Id == id));
        }

        [HttpDelete("{user_id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteRangeAsync(long user_id)
        {
            return Ok(await _service.DeleteRangeAsync(user_id));
        }
    }
}
