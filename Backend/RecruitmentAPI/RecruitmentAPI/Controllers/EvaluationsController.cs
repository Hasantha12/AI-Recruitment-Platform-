using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RecruitmentAPI.DTOs;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EvaluationsController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationsController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        // POST: api/evaluations
        [HttpPost]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> SubmitEvaluation(SubmitEvaluationDto dto)
        {
            var evaluatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _evaluationService.SubmitEvaluationAsync(dto, evaluatorId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                evaluationId = result.EvaluationId
            });
        }

        // GET: api/evaluations/application/5
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetEvaluationsForApplication(int applicationId)
        {
            var evaluations =
                await _evaluationService.GetEvaluationsForApplicationAsync(applicationId);

            return Ok(evaluations);
        }

        // ==========================================
        // Dashboard Statistics
        // GET: api/evaluations/dashboard-stats
        // ==========================================
        [HttpGet("dashboard-stats")]
        [Authorize(Roles = "HiringManager,Admin")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _evaluationService.GetDashboardStatsAsync();

            return Ok(stats);
        }
    }
}