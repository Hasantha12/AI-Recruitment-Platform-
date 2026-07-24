using RecruitmentAPI.DTOs;

namespace RecruitmentAPI.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<(bool Success, string Message, int? ApplicationId, decimal MatchScore)> ApplyToJobAsync(
            ApplyJobDto dto,
            int candidateId);

        Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(int candidateId);

        Task<List<ApplicationResponseDto>> GetApplicationsForJobAsync(int jobId);

        Task<bool> UpdateStatusAsync(int applicationId, UpdateApplicationStatusDto dto);
    }
}