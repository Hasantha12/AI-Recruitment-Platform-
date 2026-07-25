namespace RecruitmentAPI.Services.Interfaces
{
    public interface IAIService
    {
        Task<string> AnalyzeResumeAsync(string resumeText);

        Task<string> GenerateJobRecommendationAsync(
            string candidateProfile,
            string jobs);

        Task<string> GenerateFeedbackAsync(
            string candidateInfo);
    }
}