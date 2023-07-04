using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Hubs;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly IHubContext<ServerHub> _hubContext;

        public BlogPostService(IFileService fileService, AppDbContext appDbContext, IHubContext<ServerHub> hubContext)
        {
            _context = appDbContext;
            _fileService = fileService;
            _hubContext = hubContext;
        }

        public async Task<bool> CreateAsync(BlogPostCreateViewModel viewModel)
        {
            var blogPost = (BlogPost)viewModel;

            blogPost.UserId = HttpContextHelper.UserId;

            blogPost.CreatedAt = DateTime.UtcNow;

            if (viewModel.Image is not null)
                blogPost.ImagePath = await _fileService.SaveImageAsync(viewModel.Image);

            var result = await _context.BlogPosts.AddAsync(blogPost);

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceivePost", (BlogPostViewModel)result.Entity);

            return true;
        }

        public async Task DeleteAsync(Expression<Func<BlogPost, bool>> expression)
        {
            var blog = await _context.BlogPosts.FirstOrDefaultAsync(expression)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "Post not found");
            
            _context.BlogPosts.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync()
        {
            _context.BlogPosts.RemoveRange((from blog in _context.BlogPosts
                                           where blog.UserId == HttpContextHelper.UserId
                                           select blog));

            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<BlogPost, bool>>? expression = null)
        {
            expression ??= p => true;

            var results = from blog in _context.BlogPosts.Where(expression).Include(p => p.PostType).Include(p => p.User)
                    orderby blog.CreatedAt descending select ((BlogPostViewModel)blog);

            return PagedList<BlogPostViewModel>.ToPagedList(results, @params);
        }

        public async Task<PagedList<BlogPostViewModel>> GetAllByTypeIdAsync(PaginationParams @params, long id)
        {
            var results = from blog in _context.BlogPosts.Where(p => p.PostTypeId == id).Include(p => p.PostType).Include(p => p.User)
                    orderby blog.CreatedAt descending select ((BlogPostViewModel)blog);

            return PagedList<BlogPostViewModel>.ToPagedList(results, @params);
        }

        public async Task<BlogPostViewModel> GetAsync(Expression<Func<BlogPost, bool>> expression)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(expression)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            
            post.ViewCount++;

            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();

            return (BlogPostViewModel)post;
        }

        public async Task<bool> UpdateAsync(long id, BlogPostCreateViewModel viewModel)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            
            if (viewModel.Image is not null)
                post.ImagePath = await _fileService.SaveImageAsync(viewModel.Image);

            post.Title = viewModel.Title;
            post.Description = viewModel.Description;
            post.UserId = HttpContextHelper.UserId;

            post = _context.BlogPosts.Update(post).Entity;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(long id, BlogPostPatchViewModel viewModel)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            
            if (viewModel.Image is not null)
                post.ImagePath = await _fileService.SaveImageAsync(viewModel.Image);

            if (viewModel.Title is not null)
                post.Title = viewModel.Title;

            if (viewModel.PostTypeId != 0)
                post.PostTypeId = viewModel.PostTypeId;

            if (viewModel.Description is not null)
                post.Description = viewModel.Description;

            post.UserId = HttpContextHelper.UserId;

            post = _context.BlogPosts.Update(post).Entity;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
