using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.SaveMessages;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface ISaveMessageService
    {
        Task<SaveMessage> CreateAsync(SaveMessageCreateViewModel model);

        Task<bool> DeleteAsync(Expression<Func<SaveMessage, bool>> expression);

        Task<bool> DeleteRangeAsync(long userId);

        Task<IEnumerable<SaveMessage>> GetAllAsync(PaginationParams @params, Expression<Func<SaveMessage, bool>> expression = null!);
    }
}
