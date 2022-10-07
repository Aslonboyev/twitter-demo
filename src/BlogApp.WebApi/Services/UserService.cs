using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
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
                var user = await _userRepositroy.DeleteAsync(result);
                await _userRepositroy.SaveAsync();
                return user;
            }

            throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams? pagination = null, Expression<Func<User, bool>>? expression = null)
        {
            var users = _userRepositroy.GetAllAsync(expression).ToPaged(pagination);

            var userviewModel = new List<UserViewModel>();
            
            foreach (var user in users)
                userviewModel.Add((UserViewModel)user);

            return userviewModel;
        }

        

        public async Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression)
        {
            var user = await _userRepositroy.GetAsync(expression);
            
            if (user == null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            
            return (UserViewModel)user;
        }

        public async Task<UserViewModel> UpdateAsync(long id, UserCreateViewModel viewModel)
        {
            var user = await _userRepositroy.GetAsync(o => o.Id == id);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            if (viewModel.Image is not null)
                user.ImagePath = await _fileService.SaveImageAsync(viewModel.Image);



            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Email = viewModel.Email;
            user.PasswordHash = viewModel.Password;

            var updateUser = await _userRepositroy.UpdateAsync(user);

            await _userRepositroy.SaveAsync();

            return (UserViewModel)user;
        }
    }
}
