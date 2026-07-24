namespace RecruitmentAPI.DTOs
{
    public class ScheduleInterviewDto
    {
        public int ApplicationId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string? Mode { get; set; } // Online | Onsite
        public string? Notes { get; set; }
    }

    public class InterviewResponseDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public DateTime ScheduledTime { get; set; }
        public string InterviewerName { get; set; } = string.Empty;
        public string? Mode { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class SubmitEvaluationDto
    {
        public int ApplicationId { get; set; }
        public decimal Score { get; set; }
        public string? Feedback { get; set; }
        public string Decision { get; set; } = string.Empty; // Advance | Reject | Hire
    }

    public class EvaluationResponseDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string EvaluatorName { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public string? Decision { get; set; }
        public DateTime EvaluatedAt { get; set; }
    }
}
