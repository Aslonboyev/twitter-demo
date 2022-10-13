using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IFileService _fileService;

        public BlogPostService(IBlogPostRepository blogPostRepository, IFileService fileService)
        {
            _blogPostRepository = blogPostRepository;
            _fileService = fileService;
        }

        public async Task<BlogPostViewModel> CreateAsync(BlogPostCreateViewModel viewModel)
        {
            var id = HttpContextHelper.UserId;

            if (id != viewModel.UserId)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");

            var blogPost = (BlogPost)viewModel;

            blogPost.CreatedAt = DateTime.UtcNow;
            
            //if (blogPost.Image is not null)
            //    blogPost.ImagePath = await _fileService.SaveImageAsync(blogPost.Image);

            var result = await _blogPostRepository.CreateAsync(blogPost);
            await _blogPostRepository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<BlogPost, bool>> expression)
        {
            var blog = await _blogPostRepository.GetAsync(expression);

            if (blog is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Post not found");

            if (blog.Id != HttpContextHelper.UserId)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");
            
            var result = await _blogPostRepository.DeleteAsync(blog);

            await _blogPostRepository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteRangeAsync(long userId)
        {
            var blogs = _blogPostRepository.GetAll(p => p.UserId == userId);

            if(blogs is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Blogs not found");

            return await _blogPostRepository.DeleteAllAsync(blogs);
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<BlogPost, bool>>? expression = null)
        {
            return (from blog in _blogPostRepository.GetAll(expression)
                    orderby blog.CreatedAt descending
                    select (BlogPostViewModel)blog).ToPaged(@params);
        }

        public async Task<BlogPostViewModel> GetAsync(Expression<Func<BlogPost, bool>> expression)
        {
            var post = await _blogPostRepository.GetAsync(expression);
            
            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            post.ViewCount++;
            await _blogPostRepository.UpdateAsync(post);
            await _blogPostRepository.SaveAsync();
            
            return (BlogPostViewModel)post;
        }

        public async Task<BlogPostViewModel> UpdateAsync(long id, BlogPostCreateViewModel viewModel)
        {
            var post = await _blogPostRepository.GetAsync(o => o.Id == id);

            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            post.Title = viewModel.Title;
            post.Description = viewModel.Description;
            post.UserId = viewModel.UserId;

            post = await _blogPostRepository.UpdateAsync(post);
            await _blogPostRepository.SaveAsync();

            return (BlogPostViewModel)post;
        }
        
        public async Task<BlogPostViewModel> UpdateAsync(long id, BlogPostPatchViewModel viewModel)
        {
            var post = await _blogPostRepository.GetAsync(o => o.Id == id);

            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            //if(viewModel.Image is not null)
            //    post.ImagePath = await _fileService.SaveImageAsync(blogPost.Image);

            if (viewModel.Title is not null)
                post.Title = viewModel.Title;

            if (viewModel.Type is not null)
                post.Type = viewModel.Type;

            if (viewModel.Description is not null)
                post.Description = viewModel.Description;
            
            post.UserId = viewModel.UserId;

            post = await _blogPostRepository.UpdateAsync(post);
            await _blogPostRepository.SaveAsync();

            return (BlogPostViewModel)post;
        }
    }
}
