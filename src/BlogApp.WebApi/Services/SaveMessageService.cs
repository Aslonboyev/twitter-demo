using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using BlogApp.WebApi.ViewModels.SaveMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class SaveMessageService : ISaveMessageService
    {
        private readonly AppDbContext _context;

        public SaveMessageService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<SaveMessageViewModel> CreateAsync(SaveMessageCreateViewModel model)
        {
            var post = await _context.SaveMessages.FirstOrDefaultAsync(p => p.Id == model.PostId);

            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Post not found");

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == HttpContextHelper.UserId);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            var message = (SaveMessage)model;

            message.UserId = HttpContextHelper.UserId;

            var result = await _context.SaveMessages.AddAsync(message);
            await _context.SaveChangesAsync();

            return (SaveMessageViewModel)result.Entity;
        }

        public async Task DeleteAsync(Expression<Func<SaveMessage, bool>> expression)
        {
            var saveMessage = await _context.SaveMessages.FirstOrDefaultAsync(expression);

            if (saveMessage is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Saved message not found");

            var result = _context.SaveMessages.Remove(saveMessage);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync()
        {
            var messages = _context.SaveMessages.Where(p => p.UserId == HttpContextHelper.UserId);

            if (messages.IsNullOrEmpty())
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Users not found");

            _context.SaveMessages.RemoveRange(messages);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<SaveMessage, bool>> expression = null)
        {
            if (expression is null)
                expression = p => true;

            return (from save in _context.SaveMessages.Where(expression)
                    where save.UserId == HttpContextHelper.UserId
                    join blog in _context.BlogPosts
                        on save.BlogPostId equals blog.Id
                    select (BlogPostViewModel)blog).ToPaged(@params);
        }
    }
}