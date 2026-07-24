using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RecruitmentAPI.Data;
using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Services;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CandidateProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SkillExtractionService _skillService;
        private readonly IWebHostEnvironment _env;


        public CandidateProfileController(
            AppDbContext context,
            SkillExtractionService skillService,
            IWebHostEnvironment env)
        {
            _context = context;
            _skillService = skillService;
            _env = env;
        }



        // GET PROFILE
        // GET: api/CandidateProfile/me

        [HttpGet("me")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );


            var profile = await _context.CandidateProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(
                    p => p.UserId == userId
                );


            if (profile == null)
            {
                profile = new CandidateProfile
                {
                    UserId = userId
                };

                _context.CandidateProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }



            return Ok(new CandidateProfileDto
            {
                Id = profile.Id,

                UserId = profile.UserId,

                FullName =
                profile.User?.FullName ?? "",


                Headline =
                profile.Headline,


                Phone =
                profile.Phone,


                Address =
                profile.Address,


                Education =
                profile.Education,


                Certifications =
                profile.Certifications,


                ProfileImage =
                profile.ProfileImage,


                Skills =
                profile.Skills,


                ExperienceYears =
                profile.ExperienceYears,


                ResumeUrl =
                profile.ResumeUrl

            });
        }






        // UPDATE PROFILE
        // PUT: api/CandidateProfile/me


        [HttpPut("me")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UpdateMyProfile(
            UpdateProfileDto dto)
        {

            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );


            var profile =
                await _context.CandidateProfiles
                .FirstOrDefaultAsync(
                    p => p.UserId == userId
                );


            if (profile == null)
            {
                return NotFound(
                    new
                    {
                        message = "Profile not found."
                    });
            }



            profile.Headline =
                dto.Headline;


            profile.Phone =
                dto.Phone;


            profile.Address =
                dto.Address;


            profile.Education =
                dto.Education;


            profile.Certifications =
                dto.Certifications;


            profile.ExperienceYears =
                dto.ExperienceYears;



            profile.UpdatedAt =
                DateTime.UtcNow;



            await _context.SaveChangesAsync();



            return Ok(new
            {
                message =
                "Profile updated successfully."
            });

        }









        // UPLOAD PROFILE IMAGE
        // POST: api/CandidateProfile/upload-profile-image


        [HttpPost("upload-profile-image")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UploadProfileImage(
            IFormFile file)
        {


            if (file == null || file.Length == 0)
            {
                return BadRequest(
                    new
                    {
                        message = "No image uploaded."
                    });
            }



            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );



            var folder =
                Path.Combine(
                    _env.ContentRootPath,
                    "wwwroot",
                    "profile-images"
                );


            Directory.CreateDirectory(folder);



            var fileName =
                $"{userId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(file.FileName)}";



            var filePath =
                Path.Combine(
                    folder,
                    fileName
                );



            using (var stream =
                new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }



            var profile =
                await _context.CandidateProfiles
                .FirstOrDefaultAsync(
                    p => p.UserId == userId
                );



            if (profile == null)
            {
                profile = new CandidateProfile
                {
                    UserId = userId
                };

                _context.CandidateProfiles.Add(profile);
            }



            profile.ProfileImage =
                $"/profile-images/{fileName}";


            profile.UpdatedAt =
                DateTime.UtcNow;



            await _context.SaveChangesAsync();



            return Ok(new ProfileImageUploadResponseDto
            {
                Message =
                "Profile image uploaded successfully.",


                ImageUrl =
                profile.ProfileImage
            });

        }









        // UPLOAD RESUME
        // POST: api/CandidateProfile/upload-resume


        [HttpPost("upload-resume")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UploadResume(
            IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest(
                    new
                    {
                        message = "No file uploaded."
                    });
            }



            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                return BadRequest(
                    new
                    {
                        message = "Only PDF files are supported."
                    });
            }



            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );



            byte[] fileBytes;


            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }



            var folder =
                Path.Combine(
                    _env.ContentRootPath,
                    "wwwroot",
                    "resumes"
                );


            Directory.CreateDirectory(folder);



            var fileName =
                $"{userId}_{DateTime.UtcNow.Ticks}.pdf";



            var filePath =
                Path.Combine(folder, fileName);



            await System.IO.File.WriteAllBytesAsync(
                filePath,
                fileBytes
            );



            string resumeText;


            try
            {
                resumeText =
                _skillService.ExtractTextFromPdf(fileBytes);
            }
            catch
            {
                return BadRequest(
                    new
                    {
                        message = "Invalid PDF file."
                    });
            }



            var skills =
                _skillService.ExtractSkills(resumeText);



            var experience =
                _skillService.ExtractExperienceYears(resumeText);



            var profile =
                await _context.CandidateProfiles
                .FirstOrDefaultAsync(
                    p => p.UserId == userId
                );



            if (profile == null)
            {
                profile = new CandidateProfile
                {
                    UserId = userId
                };

                _context.CandidateProfiles.Add(profile);
            }



            profile.ResumeUrl =
                $"/resumes/{fileName}";


            profile.Skills =
                string.Join(", ", skills);



            if (experience.HasValue)
            {
                profile.ExperienceYears =
                    experience;
            }



            profile.UpdatedAt =
                DateTime.UtcNow;



            await _context.SaveChangesAsync();



            return Ok(new ResumeUploadResponseDto
            {
                Message =
                "Resume uploaded and analyzed successfully.",


                ExtractedSkills =
                skills,


                ExtractedExperienceYears =
                experience,


                ResumeUrl =
                profile.ResumeUrl
            });

        }

    }
}