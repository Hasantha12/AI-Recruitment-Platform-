using RecruitmentAPI.DTOs;

namespace RecruitmentAPI.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<(bool Success, string Message, int? InterviewId)> ScheduleInterviewAsync(
            ScheduleInterviewDto dto,
            int interviewerId);

        Task<List<InterviewResponseDto>> GetInterviewsForApplicationAsync(int applicationId);

        Task<List<InterviewResponseDto>> GetMyInterviewsAsync(int interviewerId);

        Task<List<InterviewResponseDto>> GetCandidateInterviewsAsync(int candidateId);

        Task<(bool Success, string Message)> UpdateInterviewAsync(
            int interviewId,
            ScheduleInterviewDto dto);

        Task<(bool Success, string Message)> CancelInterviewAsync(int interviewId);
    }
}