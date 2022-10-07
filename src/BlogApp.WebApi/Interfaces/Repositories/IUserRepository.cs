using BlogApp.WebApi.Models;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<bool> DeleteAsync(User user);

        IQueryable<User> GetAllAsync(Expression<Func<User, bool>> expression = null!);

        Task<User> GetAsync(Expression<Func<User, bool>> expression);

        Task SaveAsync();
    }
}