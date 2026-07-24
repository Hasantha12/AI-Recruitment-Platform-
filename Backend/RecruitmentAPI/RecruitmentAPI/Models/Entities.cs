using System;
using System.Collections.Generic;

namespace RecruitmentAPI.Models
{
    public enum UserRole { Candidate, Recruiter, HiringManager, Admin }
    public enum ApplicationStatus { Submitted, Shortlisted, Interviewed, Rejected, Hired }
    public enum JobStatus { Open, Closed, OnHold }

    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CandidateProfile? CandidateProfile { get; set; }
    }

    public class CandidateProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;


        public string? Headline { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Education { get; set; }

        public string? Certifications { get; set; }

        public string? ProfileImage { get; set; }


        public string? Skills { get; set; }

        public int? ExperienceYears { get; set; }

        public string? ResumeUrl { get; set; }


        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class JobPosting
    {
        public int Id { get; set; }
        public int RecruiterId { get; set; }
        public User Recruiter { get; set; } = null!;
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? RequiredSkills { get; set; }
        public string? Location { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Open;
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }

    public class Application
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public JobPosting Job { get; set; } = null!;
        public int CandidateId { get; set; }
        public User Candidate { get; set; } = null!;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
        public decimal? MatchScore { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }

    public class Interview
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
        public DateTime ScheduledTime { get; set; }
        public int InterviewerId { get; set; }
        public User Interviewer { get; set; } = null!;
        public string? Mode { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Scheduled";
    }

    public class Evaluation
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
        public int EvaluatorId { get; set; }
        public User Evaluator { get; set; } = null!;
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public string? Decision { get; set; }
        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SkillAssessment
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public User Candidate { get; set; } = null!;
        public string SkillName { get; set; } = string.Empty;
        public string? ProficiencyLevel { get; set; }
        public string? Source { get; set; }
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; }
    }
}