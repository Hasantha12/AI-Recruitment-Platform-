using RecruitmentAPI.Models;

namespace RecruitmentAPI.Repositories.Interfaces
{
    public interface IEvaluationRepository
    {
        Task<Application?> GetApplicationByIdAsync(int applicationId);

        Task AddEvaluationAsync(Evaluation evaluation);

        Task<List<Evaluation>> GetEvaluationsForApplicationAsync(int applicationId);

        Task SaveChangesAsync();

        // ============================
        // Dashboard Statistics
        // ============================

        Task<int> GetTotalEvaluationsAsync();

        Task<int> GetPendingApplicationsAsync();

        Task<int> GetHiredApplicationsAsync();

        Task<int> GetRejectedApplicationsAsync();

        Task<decimal> GetAverageScoreAsync();
    }
}