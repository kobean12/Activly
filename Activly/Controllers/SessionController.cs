using Microsoft.AspNetCore.Mvc;

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


            HttpContext.Session.SetString("UserName", model.UserName);

            var userNameInSession = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userNameInSession))
            {
                return BadRequest(new { message = "Failed to save username in session" });
            }


            return Ok(new { login = userNameInSession, message = "Login info saved in session successfully." });
        }


        [HttpGet("get")]
        public IActionResult GetUserName()
        {
            try
            {

                var userName = HttpContext.Session.GetString("UserName");

                if (string.IsNullOrEmpty(userName))
                {

                    return NotFound(new { message = "No username found in session." });
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
                HttpContext.Session.Clear(); 

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