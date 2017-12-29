using Microsoft.AspNetCore.Mvc;
using Wafer.Apis.Models;
using System.Linq;
using Wafer.Apis.Dtos.Account;
using Wafer.Apis.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Wafer.Apis.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly WaferContext _context;
        private readonly IMemoryCache _memoryCache;

        public AccountController(WaferContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public LoginInfoDto Login(string username, string password, bool rememberme)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if(user.IsNull())
            {
                return new LoginInfoDto { LoginSuccess = false };
            }

            var token = Generator.GetToken(username, password);
            _memoryCache.Set(token, token, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(5)));

            return new LoginInfoDto
            {
                LoginSuccess = true,
                Token = token,
                UserInfo = new LoginUserInfoDto
                {
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email
                }
            };
        }

        [HttpGet]
        [Route("logout")]
        public void Logout()
        {
            _memoryCache.Remove(HttpContext.Request.Headers["Authorization"]);
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