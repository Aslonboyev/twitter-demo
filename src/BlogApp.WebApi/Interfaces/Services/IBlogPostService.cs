using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IBlogPostService
    {
        Task<bool> CreateAsync(BlogPostCreateViewModel model);

        Task<BlogPostViewModel> GetAsync(Expression<Func<BlogPost, bool>> expression);

        Task DeleteAsync(Expression<Func<BlogPost, bool>> expression);

        Task<PagedList<BlogPostViewModel>> GetAllByTypeIdAsync(PaginationParams @params, long id);

        Task<bool> UpdateAsync(long id, BlogPostCreateViewModel model);

        Task<bool> UpdateAsync(long id, BlogPostPatchViewModel model);

        Task DeleteRangeAsync();

        Task<PagedList<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<BlogPost, bool>> expression = null!);
    }
}