using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.Controllers.Account
{
    public class LoginResponseDTO: BaseResponse
    {
        public string Token { get; set; }
    }
}
