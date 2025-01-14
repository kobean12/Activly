using ActivelyServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ActivelyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PinController : ControllerBase
    {

        
        private readonly string _filePath = "pins.txt";

        // POST api/pin
        [HttpPost]
        public IActionResult CreatePin([FromBody] Pin pin)
        {
            try
            {
                if (pin == null)
                {
                    return BadRequest("Pin data is null");
                }

                if (string.IsNullOrEmpty(pin.Author) || string.IsNullOrEmpty(pin.Time))
                {
                    return BadRequest("Author and Time are required fields");
                }

                string pinData = pin.ToFileFormat();

                SavePinToFile(pinData);
                SaveHeatmapCoordinatesToFile(pin.Latitude, pin.Longitude);

                return Ok(new
                {
                    Latitude = pin.Latitude,
                    Longitude = pin.Longitude,
                    Author = pin.Author,
                    Time = pin.Time,
                    Date = pin.Date,
                    Activity = pin.Activity,
                    InterestedPeople = pin.InterestedPeople
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private void SavePinToFile(string pinData)
        {
            try
            {
                System.IO.File.AppendAllText(_filePath, pinData + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving pin to file: {ex.Message}");
            }
        }

        private void SaveHeatmapCoordinatesToFile(double latitude, double longitude)
        {
            try
            {

                string filePath = Path.Combine(Directory.GetCurrentDirectory(),"heatmap.txt");


                string line = $"{latitude};{longitude}";


                System.IO.File.AppendAllText(filePath, line + Environment.NewLine);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Blad zapisu do pliku heatmap.txt: {ex.Message}");
            }
        }




        [HttpGet("heatmap")]
        public IActionResult GetHeatmapCoordinates()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "heatmap.txt");
            try
            {

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Plik heatmap.txt nie istnieje.");
                }

                var lines = System.IO.File.ReadAllLines(filePath);

                var coordinates = new List<object>();

                foreach (var line in lines)
                {
                    var parts = line.Split(';');

                    if (parts.Length == 2)
                    {
                        var coordinate = new
                        {
                            Latitude = parts[0],
                            Longitude = parts[1]  
                        };

                        coordinates.Add(coordinate);
                    }
                }

                return Ok(coordinates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }

        // GET api/pin
        [HttpGet]
        public IActionResult GetPins()
        {
            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    return Ok(new List<object>()); 
                }

                var lines = System.IO.File.ReadAllLines(_filePath);
                var pins = new List<object>();

                foreach (var line in lines)
                {
                    var parts = line.Split('/');
                    if (parts.Length >= 6) 
                    {
                        var pin = new
                        {
                            Latitude = parts[0],
                            Longitude = parts[1],
                            Author = parts[2],
                            Time = parts[3],
                            Date = parts[4],
                            Activity = parts[5],
                            InterestedPeople = parts.Skip(6).ToList() // Zainteresowani zaczynają się od 6
                        };
                        pins.Add(pin);
                    }
                }

                return Ok(pins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PUT api/pin
        [HttpPut("updatePin")]
        public IActionResult UpdatePin(double latitude, double longitude, string author, [FromBody] Pin updatedPin)
        {
            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    return NotFound("Pin file not found.");
                }

                var lines = System.IO.File.ReadAllLines(_filePath).ToList();
                bool pinFound = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    var parts = lines[i].Split('/');
                    if (parts.Length >= 6 &&
                        double.TryParse(parts[0], out double lat) &&
                        double.TryParse(parts[1], out double lng) &&
                        parts[2] == author &&
                        lat == latitude &&
                        lng == longitude)
                    {
                        lines[i] = $"{updatedPin.Latitude}/{updatedPin.Longitude}/{updatedPin.Author}/{updatedPin.Time}/{updatedPin.Activity}/{string.Join("/", updatedPin.InterestedPeople)}";
                        pinFound = true;
                        break;
                    }
                }

                if (!pinFound)
                {
                    return NotFound("Pin not found with the specified latitude, longitude, and author.");
                }

                System.IO.File.WriteAllLines(_filePath, lines);

                return Ok(new
                {
                    Latitude = updatedPin.Latitude,
                    Longitude = updatedPin.Longitude,
                    Author = updatedPin.Author,
                    Time = updatedPin.Time,
                    Activity = updatedPin.Activity,
                    InterestedPeople = updatedPin.InterestedPeople
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("addInterest")]
        public IActionResult AddInterest(double latitude, double longitude, string author, [FromBody] string user)
        {
            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    return NotFound("Pin file not found.");
                }

                var lines = System.IO.File.ReadAllLines(_filePath).ToList();
                bool pinFound = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    var parts = lines[i].Split('/');
                    if (parts.Length >= 6 &&
                        double.TryParse(parts[0], out double lat) &&
                        double.TryParse(parts[1], out double lng) &&
                        parts[2] == author &&
                        lat == latitude &&
                        lng == longitude)
                    {
                        var interests = parts.Skip(5).ToList();

                        if (!interests.Contains(user)) 
                        {
                            interests.Add(user); 
                            lines[i] = $"{parts[0]}/{parts[1]}/{parts[2]}/{parts[3]}/{parts[4]}/{string.Join("/", interests)}";
                        }

                        pinFound = true;
                        break;
                    }
                }

                if (!pinFound)
                {
                    return NotFound("Pin not found with the specified latitude, longitude, and author.");
                }

                System.IO.File.WriteAllLines(_filePath, lines);
                return Ok(new { message = "User added to the list of interested people." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("removeInterest")]
        public IActionResult RemoveInterest(double latitude, double longitude, string author, [FromBody] string user)
        {
            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    return NotFound("Pin file not found.");
                }

                var lines = System.IO.File.ReadAllLines(_filePath).ToList();
                bool pinFound = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    var parts = lines[i].Split('/');
                    if (parts.Length >= 6 &&
                        double.TryParse(parts[0], out double lat) &&
                        double.TryParse(parts[1], out double lng) &&
                        parts[2] == author &&
                        lat == latitude &&
                        lng == longitude)
                    {
                        var interests = parts.Skip(5).ToList();

                        if (interests.Contains(user)) 
                        {
                            interests.Remove(user);
                            lines[i] = $"{parts[0]}/{parts[1]}/{parts[2]}/{parts[3]}/{parts[4]}/{string.Join("/", interests)}";
                        }

                        pinFound = true;
                        break;
                    }
                }

                if (!pinFound)
                {
                    return NotFound("Pin not found with the specified latitude, longitude, and author.");
                }

                System.IO.File.WriteAllLines(_filePath, lines);
                return Ok(new { message = "User removed from the list of interested people." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
