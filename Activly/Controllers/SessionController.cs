using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Activly.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        [HttpPost("save")]
        public IActionResult SaveUserName([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName))
            {
                return BadRequest(new { message = "Invalid login data. Username is required." });
            }

            Response.Cookies.Append("UserName", model.UserName, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict 
            });

            return Ok(new { message = "Username saved in cookies successfully." });
        }

        [HttpGet("get")]
        public IActionResult GetUserName()
        {
            try
            {
                var userName = Request.Cookies["UserName"];

                if (string.IsNullOrEmpty(userName))
                {
                    return NotFound(new { message = "No username found in cookies." });
                }

                return Ok(new { login = userName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("UserName");

                return Ok(new { message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred during logout: {ex.Message}" });
            }
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
    }
}