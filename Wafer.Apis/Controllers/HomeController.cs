using Microsoft.AspNetCore.Mvc;

namespace Wafer.Apis.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Welcome to Wafer Api");
        }
    }
}