using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMatchScoreService _matchScoreService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IMatchScoreService matchScoreService)
        {
            _applicationRepository = applicationRepository;
            _matchScoreService = matchScoreService;
        }

        public async Task<(bool Success, string Message, int? ApplicationId, decimal MatchScore)> ApplyToJobAsync(
            ApplyJobDto dto,
            int candidateId)
        {
            var job = await _applicationRepository.GetJobByIdAsync(dto.JobId);

            if (job == null)
            {
                return (false, "Job not found.", null, 0);
            }

            bool alreadyApplied = await _applicationRepository.HasCandidateAppliedAsync(dto.JobId, candidateId);

            if (alreadyApplied)
            {
                return (false, "You have already applied to this job.", null, 0);
            }

            var profile = await _applicationRepository.GetCandidateProfileAsync(candidateId);

            decimal matchScore = _matchScoreService.CalculateMatchScore(
                profile?.Skills,
                job.RequiredSkills);

            var application = new Application
            {
                JobId = dto.JobId,
                CandidateId = candidateId,
                Status = ApplicationStatus.Submitted,
                MatchScore = matchScore,
                AppliedDate = DateTime.UtcNow
            };

            await _applicationRepository.AddApplicationAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return (true, "Application submitted successfully.", application.Id, matchScore);
        }

        public async Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(int candidateId)
        {
            var applications = await _applicationRepository.GetCandidateApplicationsAsync(candidateId);

            return applications.Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                CandidateId = a.CandidateId,
                CandidateName = a.Candidate.FullName,
                Status = a.Status.ToString(),
                MatchScore = a.MatchScore,
                AppliedDate = a.AppliedDate
            }).ToList();
        }

        public async Task<List<ApplicationResponseDto>> GetApplicationsForJobAsync(int jobId)
        {
            var applications = await _applicationRepository.GetApplicationsForJobAsync(jobId);

            return applications.Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                CandidateId = a.CandidateId,
                CandidateName = a.Candidate.FullName,
                Status = a.Status.ToString(),
                MatchScore = a.MatchScore,
                AppliedDate = a.AppliedDate
            }).ToList();
        }

        public async Task<bool> UpdateStatusAsync(int applicationId, UpdateApplicationStatusDto dto)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);

            if (application == null)
                return false;

            if (!Enum.TryParse<ApplicationStatus>(dto.Status, true, out var status))
                return false;

            application.Status = status;

            await _applicationRepository.UpdateApplicationAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return true;
        }
    }
}