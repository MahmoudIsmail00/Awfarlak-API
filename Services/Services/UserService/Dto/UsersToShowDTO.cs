using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService.Dto
{
    public class UsersToShowDTO
    {
        public string userId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
