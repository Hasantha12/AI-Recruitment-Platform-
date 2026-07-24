namespace RecruitmentAPI.DTOs
{
    public class AdminDashboardStatsDto
    {
        public int TotalUsers { get; set; }

        public int TotalCandidates { get; set; }

        public int TotalRecruiters { get; set; }

        public int TotalHiringManagers { get; set; }

        public int TotalOrganizations { get; set; }

        public int TotalJobs { get; set; }

        public int OpenJobs { get; set; }

        public int TotalApplications { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? OrganizationName { get; set; }
    }

    public class OrganizationResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Department { get; set; }

        public int UserCount { get; set; }

        public int JobCount { get; set; }
    }
}