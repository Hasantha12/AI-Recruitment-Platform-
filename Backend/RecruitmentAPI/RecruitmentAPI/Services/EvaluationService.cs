using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationService(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        public async Task<(bool Success, string Message, int? EvaluationId)> SubmitEvaluationAsync(
            SubmitEvaluationDto dto,
            int evaluatorId)
        {
            var application = await _evaluationRepository.GetApplicationByIdAsync(dto.ApplicationId);

            if (application == null)
            {
                return (false, "Application not found.", null);
            }

            if (!new[] { "Advance", "Reject", "Hire" }.Contains(dto.Decision))
            {
                return (false, "Decision must be Advance, Reject, or Hire.", null);
            }

            var evaluation = new Evaluation
            {
                ApplicationId = dto.ApplicationId,
                EvaluatorId = evaluatorId,
                Score = dto.Score,
                Feedback = dto.Feedback,
                Decision = dto.Decision
            };

            await _evaluationRepository.AddEvaluationAsync(evaluation);

            application.Status = dto.Decision switch
            {
                "Hire" => ApplicationStatus.Hired,
                "Reject" => ApplicationStatus.Rejected,
                _ => application.Status
            };

            await _evaluationRepository.SaveChangesAsync();

            return (true, "Evaluation submitted successfully.", evaluation.Id);
        }

        public async Task<List<EvaluationResponseDto>> GetEvaluationsForApplicationAsync(int applicationId)
        {
            var evaluations = await _evaluationRepository.GetEvaluationsForApplicationAsync(applicationId);

            return evaluations.Select(e => new EvaluationResponseDto
            {
                Id = e.Id,
                ApplicationId = e.ApplicationId,
                CandidateName = e.Application.Candidate.FullName,
                EvaluatorName = e.Evaluator.FullName,
                Score = e.Score,
                Feedback = e.Feedback,
                Decision = e.Decision,
                EvaluatedAt = e.EvaluatedAt
            }).ToList();
        }

        // ==========================================
        // Dashboard Statistics
        // ==========================================

        public async Task<EvaluationDashboardStatsDto> GetDashboardStatsAsync()
        {
            return new EvaluationDashboardStatsDto
            {
                TotalEvaluations = await _evaluationRepository.GetTotalEvaluationsAsync(),
                PendingApplications = await _evaluationRepository.GetPendingApplicationsAsync(),
                HiredApplications = await _evaluationRepository.GetHiredApplicationsAsync(),
                RejectedApplications = await _evaluationRepository.GetRejectedApplicationsAsync(),
                AverageScore = await _evaluationRepository.GetAverageScoreAsync()
            };
        }
    }
}