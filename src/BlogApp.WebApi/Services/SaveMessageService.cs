using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.BlogPosts;
using BlogApp.WebApi.ViewModels.SaveMessages;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class SaveMessageService : ISaveMessageService
    {
        private readonly IBlogPostRepository _post;
        private readonly IUserRepository _user;
        private readonly ISaveMessageRepository _repository;

        public SaveMessageService(ISaveMessageRepository repository, IUserRepository userRepository,
                                  IBlogPostRepository postRepository)
        {
            _post = postRepository;
            _user = userRepository;
            _repository = repository;
        }

        public async Task<SaveMessage> CreateAsync(SaveMessageCreateViewModel model)
        {
            var post = await _post.GetAsync(p => p.Id == model.PostId);

            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Post not found");

            var user = await _user.GetAsync(p => p.Id == model.UserId && p.ItemState == Enums.ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            var result = await _repository.CreateAsync(model);
            await _repository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<SaveMessage, bool>> expression)
        {
            var saveMessage = await _repository.GetAsync(expression);

            if (saveMessage is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Saved message not found");
            
            if (HttpContextHelper.UserId != saveMessage.UserId && HttpContextHelper.UserRole == UserRole.User.ToString())
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");

            var result = await _repository.DeleteAsync(saveMessage);

            await _repository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteRangeAsync(long userId)
        {
            if (HttpContextHelper.UserId != userId && HttpContextHelper.UserRole == UserRole.User.ToString())
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");

            var messages = _repository.GetAll(p => p.Id == userId);

            if (messages is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Users not found");

            await _repository.DeleteRangeAsync(messages);

            return true;
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetAllAsync(long id, PaginationParams @params, Expression<Func<SaveMessage, bool>> expression = null)
        {
            if (HttpContextHelper.UserId != id && HttpContextHelper.UserRole == UserRole.User.ToString())
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");


            return (from blog in _post.GetAll(p => p.UserId == id)
                         orderby blog.CreatedAt descending
                         select (BlogPostViewModel)blog).ToPaged(@params);
        }
    }
}