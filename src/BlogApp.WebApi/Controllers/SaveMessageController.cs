using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.SaveMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/save-message")]
    [ApiController]
    public class SaveMessageController : ControllerBase
    {
        private readonly ISaveMessageService _service;

        public SaveMessageController(ISaveMessageService messageService)
        {
            _service = messageService;
        }

        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateAsync([FromBody]SaveMessageCreateViewModel model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpGet("{user-id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllAsync([FromRoute(Name = "user-id")]long userId, [FromQuery] PaginationParams @params)
        {
            return Ok(await _service.GetAllAsync(userId, @params));
        }

        [HttpDelete("{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            return Ok(await _service.DeleteAsync(p => p.Id == id));
        }

        [HttpDelete("{id}/user"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteRangeAsync([FromRoute(Name = "id")] long userId)
        {
            return Ok(await _service.DeleteRangeAsync(userId));
        }
    }
}
