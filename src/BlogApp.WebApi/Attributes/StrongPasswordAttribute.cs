using BlogApp.WebApi.Validators;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return new ValidationResult("Password can not be null");
            else
            {
                string password = value.ToString()!;
                if (password.Length < 8)
                    return new ValidationResult("Password must be at least 8 characters");
                else if (password.Length > 50)
                    return new ValidationResult("Password must be less than 50 characters");
                var result = PasswordValidator.IsStrong(password);

                if (result.IsValid is false) return new ValidationResult(result.Message);
                else return ValidationResult.Success;
            }
        }
    }
}
