using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using BlogApp.WebApi.ViewModels.PostTypes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class PostTypeService : IPostTypeService
    {
        private readonly AppDbContext _context;

        public PostTypeService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<PostTypeViewModel> CreateAsync(PostTypeCreateViewModel model)
        {
            if ((await _context.PostTypes.FirstOrDefaultAsync(p => p.Name == model.Name)) is not null)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Type already exist");

            return (PostTypeViewModel)(await _context.AddAsync((PostType)model)).Entity;
        }

        public async Task<bool> DeleteAsync(Expression<Func<PostType, bool>> expression)
        {
            var result = await _context.PostTypes.FirstOrDefaultAsync(expression);

            if (result is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Type not found");

            var blogs = _context.BlogPosts.Where(p => p.PostTypeId == result.Id);

            _context.BlogPosts.RemoveRange(blogs);

            _context.PostTypes.Remove(result);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PostTypeViewModel>> GetAllAsync(PaginationParams @params)
        {
            return (from type in _context.PostTypes
                    orderby type.CreatedAt descending
                    select (PostTypeViewModel)type).ToPaged(@params);
        }

        public async Task<PostTypeViewModel> GetAsync(Expression<Func<PostType, bool>> expression)
        {
            var type = await _context.PostTypes.FirstOrDefaultAsync(expression);

            if (type is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Type not found");

            return (PostTypeViewModel)type;
        }

        public async Task<PostTypeViewModel> UpdateAsync(long id, PostTypeCreateViewModel model)
        {
            var type = await _context.PostTypes.FirstOrDefaultAsync(p => p.Id == id);

            if(type is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Type not found");

            type.Name = model.Name;

            _context.PostTypes.Update(type);

            await _context.SaveChangesAsync();

            return (PostTypeViewModel)type;
        }
    }
}
