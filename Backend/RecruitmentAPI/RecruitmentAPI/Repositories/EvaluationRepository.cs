using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;

namespace RecruitmentAPI.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly AppDbContext _context;

        public EvaluationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task AddEvaluationAsync(Evaluation evaluation)
        {
            await _context.Evaluations.AddAsync(evaluation);
        }

        public async Task<List<Evaluation>> GetEvaluationsForApplicationAsync(int applicationId)
        {
            return await _context.Evaluations
                .Include(e => e.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(e => e.Evaluator)
                .Where(e => e.ApplicationId == applicationId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ==========================================
        // Dashboard Statistics
        // ==========================================

        public async Task<int> GetTotalEvaluationsAsync()
        {
            return await _context.Evaluations.CountAsync();
        }

        public async Task<int> GetPendingApplicationsAsync()
        {
            return await _context.Applications
                .CountAsync(a =>
                    a.Status == ApplicationStatus.Submitted ||
                    a.Status == ApplicationStatus.Shortlisted ||
                    a.Status == ApplicationStatus.Interviewed);
        }

        public async Task<int> GetHiredApplicationsAsync()
        {
            return await _context.Applications
                .CountAsync(a => a.Status == ApplicationStatus.Hired);
        }

        public async Task<int> GetRejectedApplicationsAsync()
        {
            return await _context.Applications
                .CountAsync(a => a.Status == ApplicationStatus.Rejected);
        }

        public async Task<decimal> GetAverageScoreAsync()
        {
            var average = await _context.Evaluations
                .AverageAsync(e => (decimal?)e.Score);

            return average ?? 0;
        }
    }
}