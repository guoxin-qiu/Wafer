using Microsoft.AspNetCore.Mvc;
using Wafer.Apis.Models;
using System.Linq;
using Wafer.Apis.Dtos.Account;
using Wafer.Apis.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;

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
        public async Task<LoginInfoDto> Login(string username, string password, bool rememberme)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if(user.IsNull())
            {
                return new LoginInfoDto { LoginSuccess = false };
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username.ToLower()),
                new Claim(ClaimTypes.Email, user.Email.ToLower())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                StaticString.ApiCookieAuthenticationSchema);

            await HttpContext.SignInAsync(
                StaticString.ApiCookieAuthenticationSchema,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties {
                    IsPersistent = rememberme
                });

            return new LoginInfoDto
            {
                LoginSuccess = true,
                Token = Generator.GetToken(username),
                UserInfo = new LoginUserInfoDto
                {
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email
                }
            };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(StaticString.ApiCookieAuthenticationSchema);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("menu")]
        public IActionResult Get()
        {
            var menus = _context.Menus.Where(t => t.IsActive).ToList();
            return Ok(menus);
        }
    }
}