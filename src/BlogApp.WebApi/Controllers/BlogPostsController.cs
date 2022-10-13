using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebApi.Controllers
{
    [Route("api/blogposts")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _postService;

        public BlogPostsController(IBlogPostService postService)
        {
            _postService = postService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery]PaginationParams @params)
            => Ok(await _postService.GetAllAsync(@params));

        [HttpGet("{userid}/blogposts"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAllByBlogPostAsync(long userid, [FromQuery] PaginationParams @params)
            => Ok(await _postService.GetAllAsync(@params, p => p.UserId == userid));

        [HttpGet("{id}"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAsync(long id)
            => Ok(await _postService.GetAsync(p => p.Id == id));

        [HttpDelete("{userid}/blogposts"), Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteRange(long userid)
            => Ok(await _postService.DeleteRangeAsync(userid));

        [HttpDelete("{id}"), Authorize(Roles ="User, Admin")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(await _postService.DeleteAsync(p => p.Id == id)); 

        [HttpPost, Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> CreateAsync([FromForm] BlogPostCreateViewModel blogPostCreateViewModel)
            => Ok(await _postService.CreateAsync(blogPostCreateViewModel));

        [HttpPatch("{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAsync(long id, [FromForm] BlogPostCreateViewModel blogPostCreateViewModel)
            => Ok(await _postService.UpdateAsync(id, blogPostCreateViewModel));

        [HttpPut("{id}") , Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAsync(long id, [FromForm]BlogPostCreateViewModel blogPostCreateViewModel)
            => Ok(await _postService.UpdateAsync(id, blogPostCreateViewModel));
    }
}