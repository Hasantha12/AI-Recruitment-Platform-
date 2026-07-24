using RecruitmentAPI.Services;

namespace RecruitmentAPI.Services
{
    public class ResumeAnalysisService
    {
        private readonly SkillExtractionService _skillService;

        public ResumeAnalysisService(
            SkillExtractionService skillService)
        {
            _skillService = skillService;
        }


        public ResumeAnalysisResult Analyze(byte[] fileBytes)
        {

            // Extract text from PDF
            var text =
                _skillService.ExtractTextFromPdf(fileBytes);


            // Extract skills
            var skills =
                _skillService.ExtractSkills(text);


            // Extract experience
            var experience =
                _skillService.ExtractExperienceYears(text);



            return new ResumeAnalysisResult
            {
                ExtractedText = text,

                Skills = skills,

                ExperienceYears = experience
            };

        }
    }



    public class ResumeAnalysisResult
    {
        public string ExtractedText { get; set; } = "";

        public List<string> Skills { get; set; }
            = new();


        public int? ExperienceYears { get; set; }
    }
}