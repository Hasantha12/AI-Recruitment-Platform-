using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentAPI.DTOs;
using RecruitmentAPI.Services.Interfaces;
using System.Security.Claims;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        // POST: api/interviews
        [HttpPost]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> ScheduleInterview(ScheduleInterviewDto dto)
        {
            var interviewerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _interviewService.ScheduleInterviewAsync(dto, interviewerId);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                interviewId = result.InterviewId
            });
        }

        // GET: api/interviews/application/5
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetInterviewsForApplication(int applicationId)
        {
            var interviews = await _interviewService.GetInterviewsForApplicationAsync(applicationId);
            return Ok(interviews);
        }

        // GET: api/interviews/my
        [HttpGet("my")]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> GetMyInterviews()
        {
            var interviewerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var interviews = await _interviewService.GetMyInterviewsAsync(interviewerId);

            return Ok(interviews);
        }

        // GET: api/interviews/candidate/5
        [HttpGet("candidate/{candidateId}")]
        [Authorize]
        public async Task<IActionResult> GetCandidateInterviews(int candidateId)
        {
            var interviews = await _interviewService.GetCandidateInterviewsAsync(candidateId);
            return Ok(interviews);
        }

        // PUT: api/interviews/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> UpdateInterview(
            int id,
            ScheduleInterviewDto dto)
        {
            var result = await _interviewService.UpdateInterviewAsync(id, dto);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        // DELETE: api/interviews/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> CancelInterview(int id)
        {
            var result = await _interviewService.CancelInterviewAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}