using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class SecureDataController : ControllerBase
    {
        [HttpGet("GetSecureData")]
        [Authorize(Roles = "User")]
        public IActionResult GetSecureData()
        {
            return Ok(new { Message = "This is secure data" });
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "User")]
        public IActionResult GetAdmin()
        {
            return Ok(new { Message = "This is admin data" });
        }
    }
}
