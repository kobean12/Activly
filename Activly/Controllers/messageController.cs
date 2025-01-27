using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Activly2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly string _basePath;

        public MessagesController()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Notatniki");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveMessage([FromBody] MessageModel messageModel)
        {
            if (string.IsNullOrWhiteSpace(messageModel.FileName) || string.IsNullOrWhiteSpace(messageModel.Message))
            {
                return BadRequest("Nazwa pliku i wiadomość są wymagane.");
            }

            string filePath = Path.Combine(_basePath, messageModel.FileName + ".txt");

            try
            {
                await System.IO.File.AppendAllTextAsync(filePath, messageModel.Message + "\n", Encoding.UTF8);
                return Ok("Wiadomość została zapisana.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }

        [HttpGet("read")]
        public IActionResult ReadMessages([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("Nazwa pliku jest wymagana.");
            }

            string filePath = Path.Combine(_basePath, fileName + ".txt");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Plik nie istnieje.");
            }

            try
            {
                string[] content = System.IO.File.ReadAllLines(filePath);
                return Ok(content);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }
    }

    public class MessageModel
    {
        public string FileName { get; set; }
        public string Message { get; set; }
    }
}