using BlogApp.WebApi.Models;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Repositories
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task<BlogPost> UpdateAsync(BlogPost blogPost);

        Task<bool> DeleteAsync(BlogPost blogPost);

        IQueryable<BlogPost> GetAll(Expression<Func<BlogPost, bool>> expression = null!);

        Task<BlogPost> GetAsync(Expression<Func<BlogPost, bool>> expression);

        Task<bool> DeleteAllAsync(IEnumerable<BlogPost> blogs);

        Task SaveAsync();
    }
}