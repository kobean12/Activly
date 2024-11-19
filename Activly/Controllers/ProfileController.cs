using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ActivelyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        public ProfileController()
        {
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        [HttpGet("get/{userName}")]
        public IActionResult GetProfile(string userName)
        {
            var userFolder = Path.Combine(_uploadsFolder, userName);

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);

                var profileDataPath = Path.Combine(userFolder, "profileData.txt");
                var defaultProfileData = "Gender: Not set\nAge: Not set";

                System.IO.File.WriteAllText(profileDataPath, defaultProfileData);
            }

            var profileDataFilePath = Path.Combine(userFolder, "profileData.txt");
            var profileData = System.IO.File.ReadAllText(profileDataFilePath);


            var avatarPath = Path.Combine(userFolder, "avatar.png");
            string avatarUrl = null;
            if (System.IO.File.Exists(avatarPath))
            {
                avatarUrl = $"/uploads/{userName}/avatar.png";
            }

            return Ok(new
            {
                profileData,
                avatarUrl
            });
        }


        [HttpPost("update/{userName}")]
        public async Task<IActionResult> UpdateProfile(
            string userName,
            [FromForm] string gender,
            [FromForm] int age,
            [FromForm] IFormFile profilePicture)
        {
            if (string.IsNullOrEmpty(gender) || age <= 0)
            {
                return BadRequest(new { message = "Gender and age must be provided and valid." });
            }

            var userFolder = Path.Combine(_uploadsFolder, userName);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            var profileDataPath = Path.Combine(userFolder, "profileData.txt");

            var profileData = $"Gender: {gender}\nAge: {age}";
            System.IO.File.WriteAllText(profileDataPath, profileData);

            if (profilePicture != null)
            {
                var avatarPath = Path.Combine(userFolder, "avatar.png");

                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }

                using (var fileStream = new FileStream(avatarPath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }
            }
            return Ok(new { success = true, message = "Profile updated successfully" });
        }
    }
}