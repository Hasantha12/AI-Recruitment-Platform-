using RecruitmentAPI.DTOs;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // ==========================
        // Dashboard
        // ==========================

        public async Task<AdminDashboardStatsDto> GetDashboardStatsAsync()
        {
            return new AdminDashboardStatsDto
            {
                TotalUsers = await _adminRepository.GetTotalUsersAsync(),
                TotalCandidates = await _adminRepository.GetTotalCandidatesAsync(),
                TotalRecruiters = await _adminRepository.GetTotalRecruitersAsync(),
                TotalHiringManagers = await _adminRepository.GetTotalHiringManagersAsync(),
                TotalOrganizations = await _adminRepository.GetTotalOrganizationsAsync(),
                TotalJobs = await _adminRepository.GetTotalJobsAsync(),
                OpenJobs = await _adminRepository.GetOpenJobsAsync(),
                TotalApplications = await _adminRepository.GetTotalApplicationsAsync()
            };
        }

        // ==========================
        // Users
        // ==========================

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _adminRepository.GetAllUsersAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                OrganizationName = u.Organization?.Name
            }).ToList();
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);

            if (user == null)
                return false;

            user.IsActive = true;

            await _adminRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);

            if (user == null)
                return false;

            user.IsActive = false;

            await _adminRepository.SaveChangesAsync();

            return true;
        }

        // ==========================
        // Organizations
        // ==========================

        public async Task<List<OrganizationResponseDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _adminRepository.GetAllOrganizationsAsync();

            return organizations.Select(o => new OrganizationResponseDto
            {
                Id = o.Id,
                Name = o.Name,
                Department = o.Department,
                UserCount = o.Users?.Count ?? 0,
                JobCount = o.JobPostings?.Count ?? 0
            }).ToList();
        }
    }
}