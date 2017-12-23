using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Wafer.Apis.Models;
using Wafer.Apis.Dtos.Users;
using Wafer.Apis.Utils;

namespace Wafer.Apis.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly WaferContext _context;

        public UsersController(WaferContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult Get(GetUsersRequest request)
        {
            var searchText = request.SearchText;
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 9;
            var sortCol = request.SortCol;
            var sortOrder = request.SortOrder;

            var users = _context.Users.Where(t => string.IsNullOrWhiteSpace(searchText) 
            || t.Username.ToLower().Contains(searchText) 
            || t.FullName.ToLower().Contains(searchText) 
            || t.Email.ToLower().Contains(searchText));

            var totalPageCount = (users.Count() + pageSize - 1) / pageSize;

            if (sortOrder == "-")
            {
                if (sortCol == "username") users = users.OrderByDescending(t => t.Username);
                if (sortCol == "fullName") users = users.OrderByDescending(t => t.FullName);
                if (sortCol == "email") users = users.OrderByDescending(t => t.Email);
            }
            else
            {
                if (sortCol == "username") users = users.OrderBy(t => t.Username);
                if (sortCol == "fullName") users = users.OrderBy(t => t.FullName);
                if (sortCol == "email") users = users.OrderBy(t => t.Email);
            }

            users = users.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            var response = new GetUsersResponse
            {
                Success = true,
                TotalPageCount = totalPageCount,
                Users = users.Select(t => new UserInfoDto { Id = t.Id, Username = t.Username, FullName = t.FullName, Email = t.Email }).ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Post(UserInfoDto user)
        {
            var newUser = new User
            {
                Username = user.Username,
                Password = Generator.EncryptedDefaultPassword(),
                FullName = user.FullName,
                Email = user.Email
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { user = newUser });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UserInfoDto user)
        {
            var userInDb = _context.Users.FirstOrDefault(t => t.Id == id);
            if (userInDb == null) return BadRequest();

            userInDb.FullName = user.FullName;
            userInDb.Email = user.Email;
            _context.Update(userInDb);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userInDb = _context.Users.FirstOrDefault(t => t.Id == id);
            if (userInDb == null) return BadRequest();

            _context.Users.Remove(userInDb);
            _context.SaveChanges();
            return Ok();
        }
    }
}