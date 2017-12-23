using Microsoft.AspNetCore.Mvc;
using Wafer.Apis.Models;
using System.Linq;
using Wafer.Apis.Dtos.Account;
using Wafer.Apis.Utils;

namespace Wafer.Apis.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly WaferContext _context;
        public AccountController(WaferContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public LoginInfoDto Get(string username, string password, bool rememberme)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                return new LoginInfoDto
                {
                    LoginSuccess = true,
                    Token = Generator.GetToken(),
                    UserInfo = new LoginUserInfoDto
                    {
                        Username = user.Username,
                        FullName = user.FullName,
                        Email = user.Email
                    }
                };
            }
            else
            {
                return new LoginInfoDto { LoginSuccess = false };
            }
        }

        [HttpGet]
        [Route("menu")]
        public IActionResult Get()
        {
            var menus = _context.Menus.Where(t => t.IsActive).ToList();
            return Ok(menus);
        }
    }
}