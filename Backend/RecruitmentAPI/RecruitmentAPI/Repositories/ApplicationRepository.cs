using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;

namespace RecruitmentAPI.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobPosting?> GetJobByIdAsync(int jobId)
        {
            return await _context.JobPostings
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        public async Task<bool> HasCandidateAppliedAsync(int jobId, int candidateId)
        {
            return await _context.Applications
                .AnyAsync(a => a.JobId == jobId && a.CandidateId == candidateId);
        }

        public async Task<CandidateProfile?> GetCandidateProfileAsync(int candidateId)
        {
            return await _context.CandidateProfiles
                .FirstOrDefaultAsync(c => c.UserId == candidateId);
        }

        public async Task AddApplicationAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public async Task<List<Application>> GetCandidateApplicationsAsync(int candidateId)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => a.CandidateId == candidateId)
                .ToListAsync();
        }

        public async Task<List<Application>> GetApplicationsForJobAsync(int jobId)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.MatchScore)
                .ToListAsync();
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateApplicationAsync(Application application)
        {
            _context.Applications.Update(application);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}