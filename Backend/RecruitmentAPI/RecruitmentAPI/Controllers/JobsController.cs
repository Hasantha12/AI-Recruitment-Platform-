using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentAPI.DTOs;
using RecruitmentAPI.Services.Interfaces;
using System.Security.Claims;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // GET: api/jobs
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        // GET: api/jobs/recommended
        [HttpGet("recommended")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetRecommendedJobs()
        {
            var candidateId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var jobs = await _jobService.GetRecommendedJobsAsync(candidateId);

            return Ok(jobs);
        }

        // GET: api/jobs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);

            if (job == null)
                return NotFound(new { message = "Job not found." });

            return Ok(job);
        }

        // POST: api/jobs
        [HttpPost]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> CreateJob(CreateJobDto dto)
        {
            var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            int jobId = await _jobService.CreateJobAsync(dto, recruiterId);

            return Ok(new
            {
                message = "Job posted successfully.",
                jobId
            });
        }

        // PUT: api/jobs/5/close
        [HttpPut("{id}/close")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> CloseJob(int id)
        {
            bool success = await _jobService.CloseJobAsync(id);

            if (!success)
                return NotFound(new { message = "Job not found." });

            return Ok(new
            {
                message = "Job closed successfully."
            });
        }

        // DELETE: api/jobs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            bool success = await _jobService.DeleteJobAsync(id);

            if (!success)
                return NotFound(new { message = "Job not found." });

            return Ok(new
            {
                message = "Job deleted successfully."
            });
        }
    }
}