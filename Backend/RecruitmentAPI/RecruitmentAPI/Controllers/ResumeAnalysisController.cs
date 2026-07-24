using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Services;

namespace RecruitmentAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ResumeAnalysisController : ControllerBase
    {

        private readonly ResumeAnalysisService _resumeService;
        private readonly AppDbContext _context;


        public ResumeAnalysisController(
            ResumeAnalysisService resumeService,
            AppDbContext context)
        {
            _resumeService = resumeService;
            _context = context;
        }



        [HttpPost("analyze/{candidateId}")]
        public async Task<IActionResult> AnalyzeResume(
            int candidateId,
            IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest(
                "Resume file required");
            }



            using var stream = new MemoryStream();

            await file.CopyToAsync(stream);



            var result =
            _resumeService.Analyze(
                stream.ToArray()
            );



            var candidate =
            await _context.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == candidateId
            );



            if (candidate == null)
            {
                return NotFound(
                "Candidate profile not found");
            }



            candidate.Skills =
                string.Join(", ", result.Skills);



            candidate.ExperienceYears =
                result.ExperienceYears;



            candidate.ResumeUrl =
                file.FileName;



            candidate.UpdatedAt =
                DateTime.UtcNow;



            await _context.SaveChangesAsync();



            return Ok(new
            {
                message =
                "Resume analyzed and profile updated successfully.",

                skills =
                result.Skills,

                experience =
                result.ExperienceYears
            });

        }

    }

}