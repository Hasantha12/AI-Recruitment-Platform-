using RecruitmentAPI.Models;

namespace RecruitmentAPI.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task<Application?> GetApplicationByIdAsync(int applicationId);

        Task AddInterviewAsync(Interview interview);

        Task<Interview?> GetInterviewByIdAsync(int interviewId);

        Task<List<Interview>> GetInterviewsForApplicationAsync(int applicationId);

        Task<List<Interview>> GetMyInterviewsAsync(int interviewerId);

        Task<List<Interview>> GetCandidateInterviewsAsync(int candidateId);

        Task DeleteInterviewAsync(Interview interview);

        Task SaveChangesAsync();
    }
}