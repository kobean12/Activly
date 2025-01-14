using Microsoft.AspNetCore.Mvc;
using ActivelyServer.Models;
using System.Collections.Generic;
using System.Linq;
using ActivelyServer.Services;

namespace ActivelyServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CredentialsController : ControllerBase
    {
        
        private readonly ICredentialService _credentialService;

        public CredentialsController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }
        
        
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("ok"); 
        }
        

        [HttpPost]
        public ActionResult LogInUser([FromForm] string login, [FromForm] string password)
        {
            
            var credentials = _credentialService.GetAllCredentials();
            var user = credentials.Find(c => c.Login == login && c.Password == password);

            if (user != null)
            {
                return Ok(new { message = "Zalogowano pomyślnie", login = user.Login });
            }
            else
            {
                return Unauthorized(new { message = "Niepoprawny login lub hasło" });
            }
            
        }

        [HttpPut]
        public ActionResult RegisterUser([FromBody] Credentials newCredential)
        {
            
            var credentials = _credentialService.GetAllCredentials();
            var existingUser = credentials.Find(c => c.Login == newCredential.Login);

            if (existingUser != null)
            {
                return BadRequest("Użytkownik już istnieje");
            }

            _credentialService.AddCredential(newCredential);
            return Ok("Rejestracja zakończona sukcesem");  
        }
    }
}
