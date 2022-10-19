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
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.Collections;
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

        public async Task<SaveMessageViewModel> CreateAsync(SaveMessageCreateViewModel model)
        {
            var post = await _post.GetAsync(p => p.Id == model.PostId);

            if (post is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Post not found");

            var user = await _user.GetAsync(p => p.Id == HttpContextHelper.UserId && p.ItemState == Enums.ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            var message = (SaveMessage)model;

            message.UserId = HttpContextHelper.UserId;

            var result = await _repository.CreateAsync(message);
            await _repository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<SaveMessage, bool>> expression)
        {
            var saveMessage = await _repository.GetAsync(expression);

            if (saveMessage is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Saved message not found");
            
            var result = await _repository.DeleteAsync(saveMessage);

            await _repository.SaveAsync();

            return result;
        }

        public async Task<bool> DeleteRangeAsync()
        {
            var messages = _repository.GetAll(p => p.UserId == HttpContextHelper.UserId).ToList();

            if (messages.IsNullOrEmpty())
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "Users not found");

            await _repository.DeleteRangeAsync(messages);

            await _repository.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetAllAsync(PaginationParams @params, Expression<Func<SaveMessage, bool>> expression = null)
        {
            var saves =  _repository.GetAll(p => p.UserId == HttpContextHelper.UserId);

            var posts = new List<BlogPostViewModel>();

            foreach (var post in saves)
            {
                posts.Add(await _post.GetAsync(p => p.Id == post.BlogPostId));
            }

            return posts;
        }
    }
}
    