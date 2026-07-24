using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMatchScoreService _matchScoreService;

        public JobService(
            IJobRepository jobRepository,
            IMatchScoreService matchScoreService)
        {
            _jobRepository = jobRepository;
            _matchScoreService = matchScoreService;
        }

        public async Task<List<JobResponseDto>> GetAllJobsAsync()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();

            return jobs.Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                RequiredSkills = j.RequiredSkills,
                Location = j.Location,
                Status = j.Status.ToString(),
                RecruiterName = j.Recruiter.FullName,
                PostedDate = j.PostedDate,
                ApplicationCount = j.Applications.Count
            }).ToList();
        }

        public async Task<List<JobResponseDto>> GetRecommendedJobsAsync(int candidateId)
        {
            var profile = await _jobRepository.GetCandidateProfileAsync(candidateId);

            string? candidateSkills = profile?.Skills;

            var jobs = await _jobRepository.GetOpenJobsAsync();

            return jobs
                .Select(j => new JobResponseDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    RequiredSkills = j.RequiredSkills,
                    Location = j.Location,
                    Status = j.Status.ToString(),
                    RecruiterName = j.Recruiter.FullName,
                    PostedDate = j.PostedDate,
                    ApplicationCount = j.Applications.Count,
                    MatchScore = _matchScoreService.CalculateMatchScore(
                        candidateSkills,
                        j.RequiredSkills)
                })
                .OrderByDescending(j => j.MatchScore)
                .ToList();
        }

        public async Task<JobResponseDto?> GetJobByIdAsync(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);

            if (job == null)
                return null;

            return new JobResponseDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                RequiredSkills = job.RequiredSkills,
                Location = job.Location,
                Status = job.Status.ToString(),
                RecruiterName = job.Recruiter.FullName,
                PostedDate = job.PostedDate,
                ApplicationCount = job.Applications.Count
            };
        }

        public async Task<int> CreateJobAsync(CreateJobDto dto, int recruiterId)
        {
            var job = new JobPosting
            {
                Title = dto.Title,
                Description = dto.Description,
                RequiredSkills = dto.RequiredSkills,
                Location = dto.Location,
                OrganizationId = dto.OrganizationId,
                RecruiterId = recruiterId,
                Status = JobStatus.Open,
                PostedDate = DateTime.UtcNow
            };

            await _jobRepository.AddJobAsync(job);
            await _jobRepository.SaveChangesAsync();

            return job.Id;
        }

        public async Task<bool> CloseJobAsync(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);

            if (job == null)
                return false;

            job.Status = JobStatus.Closed;

            await _jobRepository.UpdateJobAsync(job);
            await _jobRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);

            if (job == null)
                return false;

            await _jobRepository.DeleteJobAsync(job);
            await _jobRepository.SaveChangesAsync();

            return true;
        }
    }
}