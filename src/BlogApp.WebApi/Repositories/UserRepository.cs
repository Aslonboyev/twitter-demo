using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbo;


        public UserRepository(AppDbContext appDbContext)
        {
            _dbo = appDbContext;
        }
        public async Task<User> CreateAsync(User user)
        {
            await _dbo.Users.AddAsync(user);
            return user;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var user = await _dbo.Users.FindAsync(id);
            if (user is not null)
            {
                _dbo.Users.Remove(user);
            }   
            return false;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            _dbo.Remove(user);
            return true;
        }

        public IQueryable<User> GetAllAsync(Expression<Func<User, bool>> expression = null)
            => expression is null ? _dbo.Users : _dbo.Users.Where(expression);

        public async Task<User?> GetAsync(Expression<Func<User, bool>> expression)
            => await _dbo.Users.FirstOrDefaultAsync(expression);

        public async Task SaveAsync()
        {
            await _dbo.SaveChangesAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbo.Update(user);
            return user;
        }
    }
}
