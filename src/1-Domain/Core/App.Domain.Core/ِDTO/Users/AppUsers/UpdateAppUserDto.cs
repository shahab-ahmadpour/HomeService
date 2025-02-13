using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core._ِDTO.Users.AppUsers
{
    public class UpdateAppUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsConfirmed { get; set; }
        public string? Password { get; set; }
    }
}
