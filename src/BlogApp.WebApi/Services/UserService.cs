﻿using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;

        public UserService(IFileService fileService, AppDbContext appDbContext)
        {
            _fileService = fileService;
            _context = appDbContext;
        }

        public async Task DeleteAsync(Expression<Func<User, bool>> expression)
        {
            var result = await _context.Users.FirstOrDefaultAsync(expression) ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            await _fileService.DeleteImageAsync(result.ImagePath);

            _context.Users.Remove(_context.Users.Where(p => p.Id == result.Id).Include(p => p.BlogPosts).First());

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == HttpContextHelper.UserId)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            await _fileService.DeleteImageAsync(user.ImagePath);

            _context.Users.Remove(_context.Users.Where(p => p.Id == user.Id).Include(p => p.BlogPosts).First());

            await _context.SaveChangesAsync();
        }

        public async Task<UserViewModel> GetInfoAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == HttpContextHelper.UserId);

            return user is null ? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found") : (UserViewModel)user;
        }

        public async Task<PagedList<UserViewModel>> GetAllAsync(PaginationParams pagination, Expression<Func<User, bool>>? expression = null)
        {
            expression ??= p => true;

            var results = from user in _context.Users.Where(expression)
                    orderby user.CreatedAt descending
                    select (UserViewModel)user;

            return PagedList<UserViewModel>.ToPagedList(results, pagination);
        }

        public async Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression)
        {
            var user = await _context.Users.FirstOrDefaultAsync(expression);

            return user == null ? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found") : (UserViewModel)user;
        }

        public async Task<UserViewModel> UpdateAsync(UserPatchViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(o => o.Id == HttpContextHelper.UserId) 
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");
            if (model.Email is not null)
            {
                var email = await _context.Users.FirstOrDefaultAsync(o => o.Email == model.Email);

                if (email is not null && user.Id != email.Id)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Email have already taken");
            }

            if (model.UserName is not null)
            {
                var username = await _context.Users.FirstOrDefaultAsync(o => o.UserName == model.UserName);

                if (username is not null && user.Id != username.Id)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Username have already taken");

                user.UserName = model.UserName;
            }

            if (model.Image is not null)
            {
                await _fileService.DeleteImageAsync(user.ImagePath);

                user.ImagePath = await _fileService.SaveImageAsync(model.Image);
            }

            if (model.FirstName is not null)
                user.FirstName = model.FirstName;

            if (model.LastName is not null)
                user.LastName = model.LastName;

            var result = _context.Users.Update(user).Entity;

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
