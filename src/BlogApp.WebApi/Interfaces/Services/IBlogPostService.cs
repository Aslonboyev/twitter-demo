using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IBlogPostService
    {
        Task<BlogPostViewModel> CreateAsync(BlogPostCreateViewModel model);

        Task<BlogPostViewModel> GetAsync(Expression<Func<BlogPost, bool>> expression);

        Task<bool> DeleteAsync(Expression<Func<BlogPost, bool>> expression);

        Task<BlogPostViewModel> UpdateAsync(long id, BlogPostCreateViewModel model);

        Task<IEnumerable<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<BlogPost, bool>> expression = null!);
    }
}