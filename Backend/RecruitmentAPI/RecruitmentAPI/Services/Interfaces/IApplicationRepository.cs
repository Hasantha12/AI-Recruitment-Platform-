using RecruitmentAPI.Models;

namespace RecruitmentAPI.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<JobPosting?> GetJobByIdAsync(int jobId);

        Task<bool> HasCandidateAppliedAsync(int jobId, int candidateId);

        Task<CandidateProfile?> GetCandidateProfileAsync(int candidateId);

        Task AddApplicationAsync(Application application);

        Task<List<Application>> GetCandidateApplicationsAsync(int candidateId);

        Task<List<Application>> GetApplicationsForJobAsync(int jobId);

        Task<Application?> GetApplicationByIdAsync(int id);

        Task UpdateApplicationAsync(Application application);

        Task SaveChangesAsync();
    }
}