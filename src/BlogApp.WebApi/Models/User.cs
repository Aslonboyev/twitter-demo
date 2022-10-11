using BlogApp.WebApi.Enums;
using BlogApp.WebApi.ViewModels.Users;

namespace BlogApp.WebApi.Models
{
    public class User
    {
        public long Id { get; set; }

        public string LastName { get; set; } = String.Empty;

        public string FirstName { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public bool IsEmailConfirmed { get; set; } = false;

        public string PasswordHash { get; set; } = String.Empty;

        public string Salt { get; set; } = String.Empty;

        public string ImagePath { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public ItemState ItemState { get; set; }
        
        public UserRole UserRole { get; set; } = UserRole.User;

        public static implicit operator UserViewModel(User user)
        {
            return new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Id = user.Id,
                ImagePath = user.ImagePath,
            };
        }
    }
}