using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Activly2.Models;

namespace Activly2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public MapController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("SavePoints")]
        public IActionResult SavePoints([FromBody] PointsModel model)
        {

            var filePath = Path.Combine(_env.WebRootPath, "points.txt");


            if (string.IsNullOrEmpty(model.Points))
            {
                return Json(new { success = false, message = "No points provided" });
            }

            try
            {

                System.IO.File.AppendAllText(filePath, model.Points + Environment.NewLine);


                return Json(new { success = true, message = "Points saved successfully" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error saving points", error = ex.Message });
            }
        }

        [HttpGet("GetPoints")]
        public IActionResult GetPoints()
        {
            var filePath = Path.Combine(_env.WebRootPath, "points.txt");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Plik z punktami nie istnieje.");
            }

            var lines = System.IO.File.ReadAllLines(filePath);

            return Ok(lines);
        }
    }
}
