using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Security;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepositroy;
        private readonly IFileService _fileService;

        public UserService(IUserRepository userRepositroy, IFileService fileService)
        {
            _userRepositroy = userRepositroy;
            _fileService = fileService;
        }

        public async Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
        {
            var result = await _userRepositroy.GetAsync(expression);

            if (result is not null)
            {
                var user = await _userRepositroy.UpdateAsync(result);

                await _userRepositroy.SaveAsync();
                return true;
            }

            throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams? pagination = null, Expression<Func<User, bool>>? expression = null)
        {
            return (from blog in _userRepositroy.GetAllAsync(expression)
                    orderby blog.CreatedAt descending
                    select (UserViewModel)blog).ToPaged(pagination);
        }

        public async Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression)
        {
            var user = await _userRepositroy.GetAsync(expression);

            if (user == null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            return (UserViewModel)user;
        }

        public async Task<bool> ImageUpdate(long id, UserImageUpdateViewModel model)
        {
            var user = await _userRepositroy.GetAsync(o => o.Id == id && o.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            if (model.Image is not null)
                user.ImagePath = await _fileService.SaveImageAsync(model.Image);

            return true;
        }

        public async Task<bool> UpdateAsync(long id, UserCreateViewModel viewModel)
        {
            var user = await _userRepositroy.GetAsync(o => o.Id == id && o.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Email = viewModel.Email;
            user.PasswordHash = PasswordHasher.ChangePassword(viewModel.Password, user.Salt);

            var updateUser = await _userRepositroy.UpdateAsync(user);

            await _userRepositroy.SaveAsync();

            return true;
        }
    }
}
