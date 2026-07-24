using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMatchScoreService _matchService;


        public RecommendationController(
            AppDbContext context,
            IMatchScoreService matchService)
        {
            _context = context;
            _matchService = matchService;
        }



        [HttpGet("{candidateId}")]
        public async Task<IActionResult> GetRecommendations(
            int candidateId)
        {


            var candidate =
            await _context.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == candidateId
            );


            if (candidate == null)
            {
                return NotFound(
                "Candidate not found");
            }



            var jobs =
            await _context.JobPostings
            .Where(x => x.Status == Models.JobStatus.Open)
            .ToListAsync();



            var recommendations =
            jobs.Select(job => new
            {

                jobId = job.Id,

                jobTitle = job.Title,

                requiredSkills = job.RequiredSkills,


                matchScore =
                _matchService.CalculateMatchScore(
                    candidate.Skills,
                    job.RequiredSkills
                )

            })
            .Where(x => x.matchScore > 0)
            .OrderByDescending(
                x => x.matchScore
            )
            .ToList();



            return Ok(recommendations);

        }

    }

}