using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;

namespace RecruitmentAPI.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        // Dashboard Statistics
        Task<int> GetTotalUsersAsync();

        Task<int> GetTotalCandidatesAsync();

        Task<int> GetTotalRecruitersAsync();

        Task<int> GetTotalHiringManagersAsync();

        Task<int> GetTotalOrganizationsAsync();

        Task<int> GetTotalJobsAsync();

        Task<int> GetOpenJobsAsync();

        Task<int> GetTotalApplicationsAsync();

        // User Management
        Task<List<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int id);

        Task SaveChangesAsync();

        // Organization Management
        Task<List<Organization>> GetAllOrganizationsAsync();
    }
}