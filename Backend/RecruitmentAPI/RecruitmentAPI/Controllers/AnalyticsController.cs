using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.Data;
using RecruitmentAPI.Models;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = new
            {
                totalUsers = await _context.Users.CountAsync(),

                totalCandidates = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Candidate),

                totalRecruiters = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Recruiter),

                totalJobs = await _context.JobPostings.CountAsync(),

                totalApplications = await _context.Applications.CountAsync(),

                shortlistedCandidates = await _context.Applications
                    .CountAsync(x => x.Status == ApplicationStatus.Shortlisted),

                hiredCandidates = await _context.Applications
                    .CountAsync(x => x.Status == ApplicationStatus.Hired),

                generatedAt = DateTime.UtcNow
            };

            return Ok(result);
        }



        [HttpGet("hiring-trend")]
        public async Task<IActionResult> GetHiringTrend()
        {
            var data = await _context.Applications
                .Where(x => x.Status == ApplicationStatus.Hired)
                .GroupBy(x => x.AppliedDate.Month)
                .Select(x => new
                {
                    month = x.Key,
                    hiredCount = x.Count()
                })
                .ToListAsync();


            return Ok(data);
        }
    }
}