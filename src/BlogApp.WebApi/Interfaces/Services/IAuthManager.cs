using BlogApp.WebApi.Models;

namespace BlogApp.WebApi.Interfaces.Services
{
    public interface IAuthManager
    {
        public string GenerateToken(User user);
    }
}
