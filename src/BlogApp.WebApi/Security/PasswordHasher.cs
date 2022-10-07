namespace BlogApp.WebApi.Security
{
    public class PasswordHasher
    {
        private const string _key = "123";
        public static (string Hash, string Salt) Hash(string password)
        {
            string salt = GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(salt + password + _key);
            return (Hash: hash, Salt: salt);
        }

        public static bool Verify(string password, string salt, string hash)
        {
            string oldhash = BCrypt.Net.BCrypt.HashPassword(salt + password + _key);
            return oldhash == hash;
        }

        public static string ChangePassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(salt + password + _key);
        }

        private static string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
