﻿using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (_extensions.Contains(extension.ToLower()))
                    return ValidationResult.Success;
                else return new ValidationResult("This file extension is not supperted!");
            }
            else return ValidationResult.Success;
        }
    }
}