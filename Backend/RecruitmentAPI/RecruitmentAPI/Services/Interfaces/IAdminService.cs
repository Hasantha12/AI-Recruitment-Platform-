using RecruitmentAPI.DTOs;

namespace RecruitmentAPI.Services.Interfaces
{
    public interface IAdminService
    {
        // Dashboard
        Task<AdminDashboardStatsDto> GetDashboardStatsAsync();

        // Users
        Task<List<UserResponseDto>> GetAllUsersAsync();

        Task<bool> ActivateUserAsync(int userId);

        Task<bool> DeactivateUserAsync(int userId);

        // Organizations
        Task<List<OrganizationResponseDto>> GetAllOrganizationsAsync();
    }
}