﻿using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostService(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPostViewModel> CreateAsync(BlogPostCreateViewModel viewModel)
        {
            var blogPost = (BlogPost)viewModel;

            blogPost.CreatedAt = DateTime.UtcNow;

            var result = await _blogPostRepository.CreateAsync(blogPost);
            await _blogPostRepository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<BlogPost, bool>> expression)
        {
            var blog = await _blogPostRepository.GetAsync(expression);

            if (blog is not null)
            {
                var result = await _blogPostRepository.DeleteAsync(blog);

                await _blogPostRepository.SaveAsync();

                return result;
            }

            return false;
        }

        public async Task<bool> DeleteRangeAsync(long userId)
        {
            var blogs = _blogPostRepository.GetAll(p => p.UserId == userId);

            if(blogs is not null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Blogs not found");

            return await _blogPostRepository.DeleteAllAsync(blogs);
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<BlogPost, bool>>? expression = null)
        {
            if(@params.PageIndex == 0 && @params.PageSize == 0 || @params is null)
            {
                var blogs = _blogPostRepository.GetAll(expression).OrderBy(o => o.CreatedAt);

                var blogPosts = new List<BlogPostViewModel>();

                foreach (var blog in blogs)
                    blogPosts.Add((BlogPostViewModel)blog);

                return blogPosts;
            }

            var posts = _blogPostRepository.GetAll(expression).OrderBy(p => p.CreatedAt).ToPaged(@params);

            if(posts is null)
                return Enumerable.Empty<BlogPostViewModel>();

            var blogViews = new List<BlogPostViewModel>();

            foreach (var post in posts)
                blogViews.Add((BlogPostViewModel)post);

            return blogViews;
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
            post.UpdatedAt = DateTime.UtcNow;

            post = await _blogPostRepository.UpdateAsync(post);
            await _blogPostRepository.SaveAsync();

            return (BlogPostViewModel)post;
        }
    }
}
