using RecruitmentAPI.DTOs;

namespace RecruitmentAPI.Services.Interfaces
{
    public interface IJobService
    {
        Task<List<JobResponseDto>> GetAllJobsAsync();

        Task<List<JobResponseDto>> GetRecommendedJobsAsync(int candidateId);

        Task<JobResponseDto?> GetJobByIdAsync(int id);

        Task<int> CreateJobAsync(CreateJobDto dto, int recruiterId);

        Task<bool> CloseJobAsync(int id);

        Task<bool> DeleteJobAsync(int id);
    }
}