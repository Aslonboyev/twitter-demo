using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.PostTypes;
using System.Linq.Expressions;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IPostTypeService
    {
        Task<PostTypeViewModel> CreateAsync(PostTypeCreateViewModel model);

        Task<PostTypeViewModel> GetAsync(Expression<Func<PostType, bool>> expression);

        Task DeleteAsync(Expression<Func<PostType, bool>> expression);

        Task<PostTypeViewModel> UpdateAsync(long id, PostTypeCreateViewModel model);

        Task<PagedList<PostTypeViewModel>> GetAllAsync(PaginationParams @params);
    }
}
