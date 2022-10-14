using BlogApp.WebApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.Attributes
{/// <summary>
 /// File size is measured in 'MB'
 /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                if (FileSizeHelper.ByteToMB(file.Length) > _maxFileSize)
                    return new ValidationResult($"Image must be less than {_maxFileSize} MB");
                else return ValidationResult.Success;
            }
            else return ValidationResult.Success;
        }
    }
}
