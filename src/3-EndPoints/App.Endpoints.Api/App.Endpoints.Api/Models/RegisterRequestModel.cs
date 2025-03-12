using System.ComponentModel.DataAnnotations;

namespace App.Endpoints.Api.Models
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل نامعتبر است")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public string Password { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "نقش کاربر الزامی است")]
        public string Role { get; set; }
    }
}