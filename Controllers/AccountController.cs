using CookbookBE.Controllers.Account;
using CookbookBE.DataLayer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookbookBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public IConfiguration _config { get; }

        public AccountController(ApplicationContext db, IConfiguration config)
        {
            _config = config;
            _db = db;
        }

        [HttpPost("Register")]
        public RegisterResponseDTO Register([FromBody] RegisterRequestDTO payload)
        {

            var possibleAlreadyExistingUser = _db.Users.SingleOrDefault(u => u.Email == payload.Email);
            if (possibleAlreadyExistingUser != null) {
                return new RegisterResponseDTO
                {
                    Error = "This Email already exists in the system!"
                };
            }
            var newUser = new DataLayer.Entities.User
            {
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                Email = payload.Email,
                PasswordHash = HashPassword(payload.Password),
      
            };
            _db.Users.Add(newUser);

            _db.SaveChanges();
            return new RegisterResponseDTO()
            {
                Token = GenerateJSONWebToken(newUser),
            };
        }

        [HttpPost("Login")]
        public LoginResponseDTO Login([FromBody] LoginRequestDTO payload)
        {
            var existingUser = _db.Users.SingleOrDefault(u => u.Email == payload.Email);
            if (existingUser != null)
            {
                if (existingUser.PasswordHash == payload.Password)
                {

                }
                else
                    return new LoginResponseDTO()
                    {
                        Error = "The password is incorrect"
                    };
            }
            else
                return new LoginResponseDTO()
                {
                    Error = "The user is incorrect or doesn't exist"
                };

            return new LoginResponseDTO();
        }

        #region SecurityHelpers
        private string HashPassword(string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = BitConverter.GetBytes(RandomNumberGenerator.GetInt32(1521841412));

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private string GenerateJSONWebToken(DataLayer.Entities.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("LastLoginDate", DateTime.Now.ToString()),
                new Claim("Id", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(null,
              null,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

#endregion
    }
}
