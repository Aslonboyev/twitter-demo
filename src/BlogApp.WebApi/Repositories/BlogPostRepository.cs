using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly DbSet<BlogPost> _dbSet;

        public BlogPostRepository(AppDbContext appDb)
        {
            appDbContext = appDb;
            _dbSet = appDbContext.Set<BlogPost>();
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            var post = await _dbSet.AddAsync(blogPost);

            return post.Entity;
        }

        public async Task<bool> DeleteAsync(BlogPost blogPost)
        {
            _dbSet.Remove(blogPost);

            return true;
        }

        public IQueryable<BlogPost> GetAll(Expression<Func<BlogPost, bool>>? expression = null)
        {
            return expression is null ? _dbSet : _dbSet.Where(expression);
        }

        public Task<BlogPost?> GetAsync(Expression<Func<BlogPost, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task SaveAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            return _dbSet.Update(blogPost).Entity;
        }
    }
}
