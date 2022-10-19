using BlogApp.WebApi.Enums;

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

    }
}