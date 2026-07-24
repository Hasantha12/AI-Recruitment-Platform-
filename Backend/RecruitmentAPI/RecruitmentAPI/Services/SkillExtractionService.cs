using UglyToad.PdfPig;

namespace RecruitmentAPI.Services
{
    public class SkillExtractionService
    {
        // Known skills database — the extraction engine matches resume text against this taxonomy.
        // In a production system this would be a much larger, database-backed skill ontology.
        private static readonly string[] KnownSkills = new[]
        {
            "C#", "Java", "Python", "JavaScript", "TypeScript", "SQL", "HTML", "CSS",
            "Angular", "React", "Vue", "Node.js", "ASP.NET", ".NET", "Spring Boot",
            "SQL Server", "MySQL", "PostgreSQL", "MongoDB", "Firebase",
            "Git", "Docker", "Kubernetes", "Azure", "AWS", "CI/CD", "Jenkins",
            "REST API", "GraphQL", "Microservices", "Agile", "Scrum",
            "Machine Learning", "Data Analysis", "Communication", "Leadership",
            "Project Management", "Problem Solving", "Teamwork", "Testing",
            "Unit Testing", "Entity Framework", "jQuery", "Bootstrap", "Tailwind",
            "PHP", "C++", "Swift", "Kotlin", "Flutter", "Django", "Flask",
            "Excel", "PowerBI", "Tableau", "Linux", "Windows Server", "Networking"
        };

        /// Extracts raw text from an uploaded PDF resume.
        public string ExtractTextFromPdf(byte[] fileBytes)
        {
            var text = new System.Text.StringBuilder();
            using var document = PdfDocument.Open(fileBytes);
            foreach (var page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
            return text.ToString();
        }

        /// AI-assisted skill extraction: scans resume text against the known skills
        /// taxonomy using case-insensitive whole-word matching (NLP keyword-spotting technique).
        public List<string> ExtractSkills(string resumeText)
        {
            if (string.IsNullOrWhiteSpace(resumeText)) return new List<string>();

            var found = new List<string>();
            var normalizedText = resumeText.ToLowerInvariant();

            foreach (var skill in KnownSkills)
            {
                var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(skill.ToLowerInvariant())}\b";
                if (System.Text.RegularExpressions.Regex.IsMatch(normalizedText, pattern))
                {
                    found.Add(skill);
                }
            }

            return found.Distinct().ToList();
        }

        /// Estimates years of experience by scanning for patterns like "3 years", "5+ years".
        public int? ExtractExperienceYears(string resumeText)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                resumeText, @"(\d+)\+?\s*(years|yrs)\s*(of)?\s*experience",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (match.Success && int.TryParse(match.Groups[1].Value, out int years))
                return years;

            return null;
        }
    }
}