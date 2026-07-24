using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;

namespace RecruitmentAPI.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly AppDbContext _context;

        public InterviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task<Interview?> GetInterviewByIdAsync(int interviewId)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Interviewer)
                .FirstOrDefaultAsync(i => i.Id == interviewId);
        }

        public async Task AddInterviewAsync(Interview interview)
        {
            await _context.Interviews.AddAsync(interview);
        }

        public async Task<List<Interview>> GetInterviewsForApplicationAsync(int applicationId)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Interviewer)
                .Where(i => i.ApplicationId == applicationId)
                .OrderBy(i => i.ScheduledTime)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetMyInterviewsAsync(int interviewerId)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Interviewer)
                .Where(i => i.InterviewerId == interviewerId)
                .OrderBy(i => i.ScheduledTime)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetCandidateInterviewsAsync(int candidateId)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Interviewer)
                .Where(i => i.Application.CandidateId == candidateId)
                .OrderBy(i => i.ScheduledTime)
                .ToListAsync();
        }

        public async Task DeleteInterviewAsync(Interview interview)
        {
            _context.Interviews.Remove(interview);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}