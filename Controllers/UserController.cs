using CookbookBE.Controllers.User;
using CookbookBE.DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CookbookBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public UserController(ApplicationContext db)
        {
            _db = db;
        }

        // nu am stiut
        [HttpGet("DownloadProfilePicture")]
        public async Task<FileContentResult> DownloadProfilePictureAsync(string filename)
        {
            var path = Path.GetFullPath("./wwwroot/images/school-assets/" + filename);
            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "image/png", Path.GetFileName(path));
        }

        // nu am stiut
        [HttpPost("UploadProfilePicture/{id:int}")]
        public ActionResult UploadProfilePicture(int id)
        {
            try
            {
                // ...

                var picture = Request.Form.Files[0];

                

                // ...

                return Ok();
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetUserProfile/{id:int}")] 
        public ActionResult<User> GetUserProfile(int id)
        {
            return _db.Users.Where(user => id == user.Id).Single();
        }

        [HttpPut("UpdateUser/{id:int}")]
        public ActionResult<UserDTO> UpdateUser(int id, [FromBody] UserDTO payload)
        {

            var userToEdit = _db.Users.FirstOrDefault(x => x.Id == id);
            if (userToEdit != null)
            {
                userToEdit.FirstName = payload.FullName;
                userToEdit.Email = payload.Email;
                _db.SaveChanges();

                return Ok(userToEdit);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteUser/{id:int}")]
        public ActionResult<bool> Delete(int id)
        {
            try
            {
                var userToDelete = _db.Users.Single(user => id == user.Id);

                _db.Users.Remove(userToDelete);
                _db.SaveChanges();
                return Ok(new { status = true });
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("AllUsers")] 
        public ActionResult<GetAllUsersResponse> GetAllUsers(int pageSize, int pageNumber, Enums.SortType sortType)
        {
            var allUsersQuery = _db.Users.AsQueryable();

            switch (sortType)
            {
                case Enums.SortType.FirstNameAscending:
                    allUsersQuery = allUsersQuery.OrderBy(x => x.FirstName);
                    break;
                case Enums.SortType.FirstNameDescending:
                    allUsersQuery = allUsersQuery.OrderByDescending(x => x.FirstName);
                    break;
                case Enums.SortType.LastNameAscending:
                    allUsersQuery = allUsersQuery.OrderBy(x => x.LastName);
                    break;
                case Enums.SortType.LastNameDescending:
                    allUsersQuery = allUsersQuery.OrderByDescending(x => x.LastName);
                    break;
                default: throw new ApplicationException("Unknown sort type");
            }

            var allUsers = allUsersQuery
                .Select(u => new UserDTO
                {
                    FullName = u.FirstName + ' ' + u.LastName,
                    Email = u.Email,
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return Ok(new GetAllUsersResponse
            {
                Users = allUsers,
            });
        }

        private string GetCurrentUserEmail()
        {
            return HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?
                .Value;
        }
    }


}
