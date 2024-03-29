﻿namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile image);

        Task DeleteImageAsync(string imagePath);
    }
}
