using BlogApp.WebApi.Models;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Repositories
{
    public interface ISaveMessageRepository
    {
        Task<SaveMessage> CreateAsync(SaveMessage saveMessage);

        Task<SaveMessage> UpdateAsync(SaveMessage saveMessage);

        Task<bool> DeleteAsync(SaveMessage saveMessage);
        
        IQueryable<SaveMessage> GetAll(Expression<Func<SaveMessage, bool>> expression = null!);

        Task<SaveMessage> GetAsync(Expression<Func<SaveMessage, bool>> expression);

        Task SaveAsync();

        Task<bool> DeleteRangeAsync(IEnumerable<SaveMessage> messages);
    }
}
