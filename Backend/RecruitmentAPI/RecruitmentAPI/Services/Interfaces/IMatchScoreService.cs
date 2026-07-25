namespace RecruitmentAPI.Services.Interfaces
{
    public interface IMatchScoreService
    {
        decimal CalculateMatchScore(string? candidateSkills, string? requiredSkills);
    }
}