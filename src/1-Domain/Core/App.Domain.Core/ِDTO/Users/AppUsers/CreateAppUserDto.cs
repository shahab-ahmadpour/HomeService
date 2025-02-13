using App.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core._ِDTO.Users.AppUsers
{
    public class CreateAppUserDto
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string ProfilePicture { get; set; } = "default.png";

        [Required]
        public UserRole Role { get; set; }

        public bool IsEnabled { get; set; } = false;

        public bool IsConfirmed { get; set; } = false;
    }
}
