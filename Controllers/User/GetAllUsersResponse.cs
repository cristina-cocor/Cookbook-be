using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.Controllers.User
{
    public class GetAllUsersResponse
    {
        public List<UserDTO> Users { get; set; }
    }

    public class UserDTO
    {
        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
