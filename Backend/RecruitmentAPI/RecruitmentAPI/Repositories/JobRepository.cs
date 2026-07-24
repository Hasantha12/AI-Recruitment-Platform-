using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;

namespace RecruitmentAPI.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all jobs
        public async Task<IEnumerable<JobPosting>> GetAllJobsAsync()
        {
            return await _context.JobPostings
                .Include(j => j.Recruiter)
                .Include(j => j.Applications)
                .ToListAsync();
        }

        // Get only open jobs
        public async Task<IEnumerable<JobPosting>> GetOpenJobsAsync()
        {
            return await _context.JobPostings
                .Include(j => j.Recruiter)
                .Include(j => j.Applications)
                .Where(j => j.Status == JobStatus.Open)
                .ToListAsync();
        }

        // Get one job by ID
        public async Task<JobPosting?> GetJobByIdAsync(int id)
        {
            return await _context.JobPostings
                .Include(j => j.Recruiter)
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        // Get candidate profile
        public async Task<CandidateProfile?> GetCandidateProfileAsync(int userId)
        {
            return await _context.CandidateProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Add new job
        public async Task AddJobAsync(JobPosting job)
        {
            await _context.JobPostings.AddAsync(job);
        }

        // Update job
        public async Task UpdateJobAsync(JobPosting job)
        {
            _context.JobPostings.Update(job);
            await Task.CompletedTask;
        }

        // Delete job
        public async Task DeleteJobAsync(JobPosting job)
        {
            _context.JobPostings.Remove(job);
            await Task.CompletedTask;
        }

        // Save changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
