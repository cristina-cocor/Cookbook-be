using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.Controllers.Account
{
    public class RegisterResponseDTO: BaseResponse
    {
        public string Token { get; set; }
    }
}
