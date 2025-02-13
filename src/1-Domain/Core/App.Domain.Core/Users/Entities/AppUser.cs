using App.Domain.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Users.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; } = 0;
        public string ProfilePicture { get; set; } = "default.png";
        public bool IsEnabled { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public UserRole Role { get; set; }

        public Customer? Customer { get; set; }
        public Expert? Expert { get; set; }
        public Admin? Admin { get; set; }
    }
}