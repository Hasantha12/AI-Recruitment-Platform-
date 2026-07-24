using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class MatchScoreService : IMatchScoreService
    {
        public decimal CalculateMatchScore(string? candidateSkills, string? requiredSkills)
        {
            if (string.IsNullOrWhiteSpace(candidateSkills) ||
                string.IsNullOrWhiteSpace(requiredSkills))
            {
                return 0;
            }

            var candidateSet = candidateSkills
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToLowerInvariant())
                .ToHashSet();

            var requiredSet = requiredSkills
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToLowerInvariant())
                .ToHashSet();

            if (requiredSet.Count == 0)
                return 0;

            int matches = requiredSet.Count(skill => candidateSet.Contains(skill));

            decimal coverageScore =
                (decimal)matches / requiredSet.Count * 100;

            int extraSkills = candidateSet.Intersect(requiredSet).Count();

            decimal bonusScore =
                candidateSet.Count > 0
                ? Math.Min(extraSkills * 2, 10)
                : 0;

            return Math.Round(
                Math.Min(coverageScore + bonusScore, 100),
                2);
        }
    }
}