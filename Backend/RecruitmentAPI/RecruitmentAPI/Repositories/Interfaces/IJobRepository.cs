using RecruitmentAPI.Models;

namespace RecruitmentAPI.Repositories.Interfaces
{
    public interface IJobRepository
    {
        // Get all jobs
        Task<IEnumerable<JobPosting>> GetAllJobsAsync();

        // Get only open jobs
        Task<IEnumerable<JobPosting>> GetOpenJobsAsync();

        // Get single job
        Task<JobPosting?> GetJobByIdAsync(int id);

        // Get candidate profile
        Task<CandidateProfile?> GetCandidateProfileAsync(int userId);

        // Add new job
        Task AddJobAsync(JobPosting job);

        // Update existing job
        Task UpdateJobAsync(JobPosting job);

        // Delete job
        Task DeleteJobAsync(JobPosting job);

        // Save database changes
        Task SaveChangesAsync();
    }
}