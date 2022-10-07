using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlogApp.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return new ValidationResult("Name can not be null");
            else
            {
                string name = value.ToString()!;

                Regex regex = new Regex(@"^(?=.{1,40}$)[a-zA-Z]+(?:[-'\s][a-zA-Z]+)*$");

                if (regex.Match(name).Success)
                    return ValidationResult.Success;

                return new ValidationResult("Please enter valid first name.\nFirst name must be contains only letters or ' character");
            }
        }
    }
}
