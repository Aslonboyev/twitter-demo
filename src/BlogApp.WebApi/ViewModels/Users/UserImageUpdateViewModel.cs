using BlogApp.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Service.ViewModels.Users
{
    public class UserImageUpdateViewModel
    {
        [Required(ErrorMessage = "Image is required")]
        [DataType(DataType.Upload)]
        [MaxFileSize(3)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; } = null!;
    }
}
