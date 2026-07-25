using RecruitmentAPI.DTOs;

namespace RecruitmentAPI.Services.Interfaces
{
    public interface IEvaluationService
    {
        Task<(bool Success, string Message, int? EvaluationId)> SubmitEvaluationAsync(
            SubmitEvaluationDto dto,
            int evaluatorId);

        Task<List<EvaluationResponseDto>> GetEvaluationsForApplicationAsync(int applicationId);

        // ============================
        // Dashboard Statistics
        // ============================

        Task<EvaluationDashboardStatsDto> GetDashboardStatsAsync();
    }

    public class EvaluationDashboardStatsDto
    {
        public int TotalEvaluations { get; set; }

        public int PendingApplications { get; set; }

        public int HiredApplications { get; set; }

        public int RejectedApplications { get; set; }

        public decimal AverageScore { get; set; }
    }
}