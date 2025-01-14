using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Globalization;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        public class ActivityRequest
        {
            public string Activity { get; set; }
            public string Time { get; set; }
            public string TimeRange { get; set; }
            public string User { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private const string FilePath = "activities.txt";

        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity([FromBody] ActivityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Activity) ||
                string.IsNullOrWhiteSpace(request.Time) ||
                string.IsNullOrWhiteSpace(request.TimeRange) ||
                string.IsNullOrWhiteSpace(request.User) ||
                request.Latitude == 0 ||
                request.Longitude == 0)
            {
                return BadRequest("All fields are required.");
            }

            string line = $"{request.Activity};{request.Time};{request.TimeRange};{request.User};{request.Latitude};{request.Longitude}";

            try
            {
                System.IO.File.AppendAllText(FilePath, line + Environment.NewLine);
                return Ok("Activity logged successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllActivities")]
        public IActionResult GetAllActivities()
        {
            try
            {
                if (!System.IO.File.Exists(FilePath))
                {
                    return NotFound("No activities found.");
                }

                string fileContent = System.IO.File.ReadAllText(FilePath, Encoding.UTF8);

                return Ok(fileContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading file: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("CheckProximity")]
        public IActionResult CheckProximity([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] string activity)
        {
            try
            {
                if (!System.IO.File.Exists(FilePath))
                {
                    return NotFound("No activities found.");
                }

                var lines = System.IO.File.ReadAllLines(FilePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length < 6) continue;

                    var existingActivity = parts[0];
                    if (!existingActivity.Equals(activity, StringComparison.OrdinalIgnoreCase)) continue;

                    if (double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var existingLat) &&
                        double.TryParse(parts[5], NumberStyles.Float, CultureInfo.InvariantCulture, out var existingLng))
                    {
                        if (IsWithinRadius(latitude, longitude, existingLat, existingLng, 1))
                        {
                            return Ok("Ok");
                        }
                    }
                }

                return NotFound("No matching activity found within 1 km.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error checking proximity: {ex.Message}");
            }
        }

        private bool IsWithinRadius(double lat1, double lng1, double lat2, double lng2, double radiusKm)
        {
            const double EarthRadiusKm = 6371.0;
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLng = DegreesToRadians(lng2 - lng1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadiusKm * c;

            return distance <= radiusKm;
        }
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}