using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression = null);

        Task<bool> DeleteAsync(Expression<Func<User, bool>> expression);
        
        Task<UserViewModel> GetInfoAsync();

        Task<bool> DeleteAsync();

        //Task<bool> UpdateAsync(long id, UserCreateViewModel model);

        Task<bool> UpdateAsync(UserPatchViewModel model);

        Task<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null!);
    }
}
