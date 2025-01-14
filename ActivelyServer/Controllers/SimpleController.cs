using Microsoft.AspNetCore.Mvc;

namespace ActivelyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("ok"); 
        }
    }
}