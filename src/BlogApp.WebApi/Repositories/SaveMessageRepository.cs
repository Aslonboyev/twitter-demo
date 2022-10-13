using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Repositories
{
    public class SaveMessageRepository : ISaveMessageRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly DbSet<SaveMessage> _dbSet;

        public SaveMessageRepository(AppDbContext context)
        {
            appDbContext = context;
            _dbSet = appDbContext.Set<SaveMessage>();
        }

        public async Task<SaveMessage> CreateAsync(SaveMessage saveMessage)
        {
            var post = await _dbSet.AddAsync(saveMessage);

            return post.Entity;
        }

        public async Task<bool> DeleteAsync(SaveMessage saveMessage)
        {
            _dbSet.Remove(saveMessage);

            return true;
        }

        public async Task<bool> DeleteAllAsync(IEnumerable<SaveMessage> saveMessages)
        {
            _dbSet.RemoveRange(saveMessages);

            return true;
        }

        public IQueryable<SaveMessage> GetAll(Expression<Func<SaveMessage, bool>>? expression = null)
        {
            return expression is null ? _dbSet : _dbSet.Where(expression);
        }

        public async Task<SaveMessage?> GetAsync(Expression<Func<SaveMessage, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task SaveAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async Task<SaveMessage> UpdateAsync(SaveMessage saveMessage)
        {
            return _dbSet.Update(saveMessage).Entity;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<SaveMessage> messages)
        {
            _dbSet.RemoveRange(messages);

            return true;
        }
    }
}
