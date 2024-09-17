using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService.Dto
{
    public class UserChangePasswordDto
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
