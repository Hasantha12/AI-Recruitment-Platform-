namespace RecruitmentAPI.DTOs
{
    public class CreateJobDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? RequiredSkills { get; set; }
        public string? Location { get; set; }
        public int OrganizationId { get; set; }
    }

    public class JobResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? RequiredSkills { get; set; }
        public string? Location { get; set; }
        public string Status { get; set; } = string.Empty;
        public string RecruiterName { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public int ApplicationCount { get; set; }
        public decimal? MatchScore { get; set; }  // AI-calculated skill-match % (only populated for recommended jobs)
    }

    public class ApplyJobDto
    {
        public int JobId { get; set; }
    }

    public class ApplicationResponseDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? MatchScore { get; set; }
        public DateTime AppliedDate { get; set; }
    }

    public class UpdateApplicationStatusDto
    {
        public string Status { get; set; } = string.Empty; // Shortlisted, Interviewed, Rejected, Hired
    }
}