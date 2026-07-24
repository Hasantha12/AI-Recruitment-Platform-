namespace RecruitmentAPI.DTOs
{
    public class CandidateProfileDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }


        // User Information
        public string FullName { get; set; } = string.Empty;


        // Professional Profile
        public string? Headline { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Education { get; set; }

        public string? Certifications { get; set; }

        public string? ProfileImage { get; set; }


        // AI Resume Information
        public string? Skills { get; set; }

        public int? ExperienceYears { get; set; }

        public string? ResumeUrl { get; set; }
    }



    public class UpdateProfileDto
    {

        public string? Headline { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Education { get; set; }

        public string? Certifications { get; set; }

        public int? ExperienceYears { get; set; }

    }




    public class ProfileImageUploadResponseDto
    {

        public string Message { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

    }




    public class ResumeUploadResponseDto
    {

        public string Message { get; set; } = string.Empty;


        public List<string> ExtractedSkills { get; set; } = new();


        public int? ExtractedExperienceYears { get; set; }


        public string ResumeUrl { get; set; } = string.Empty;

    }
}