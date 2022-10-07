using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression);

        Task<bool> DeleteAsync(Expression<Func<User, bool>> expression);

        Task<bool> UpdateAsync(ulong id, UserCreateViewModel model);

        Task<bool> ImageUpdate(ulong id, UserImageUpdateViewModel model);

        Task<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null!);
    }
}
