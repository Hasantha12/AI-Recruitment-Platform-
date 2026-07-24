using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;

namespace RecruitmentAPI.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        // ===========================
        // Dashboard Statistics
        // ===========================

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalCandidatesAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Role == UserRole.Candidate);
        }

        public async Task<int> GetTotalRecruitersAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Role == UserRole.Recruiter);
        }

        public async Task<int> GetTotalHiringManagersAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Role == UserRole.HiringManager);
        }

        public async Task<int> GetTotalOrganizationsAsync()
        {
            return await _context.Organizations.CountAsync();
        }

        public async Task<int> GetTotalJobsAsync()
        {
            return await _context.JobPostings.CountAsync();
        }

        public async Task<int> GetOpenJobsAsync()
        {
            return await _context.JobPostings
                .CountAsync(j => j.Status == JobStatus.Open);
        }

        public async Task<int> GetTotalApplicationsAsync()
        {
            return await _context.Applications.CountAsync();
        }

        // ===========================
        // User Management
        // ===========================

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Organization)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ===========================
        // Organization Management
        // ===========================

        public async Task<List<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations
                .Include(o => o.Users)
                .Include(o => o.JobPostings)
                .OrderBy(o => o.Name)
                .ToListAsync();
        }
    }
}