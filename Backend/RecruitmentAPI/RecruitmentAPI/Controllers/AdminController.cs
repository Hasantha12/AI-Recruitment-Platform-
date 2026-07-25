using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // ==========================
        // Dashboard
        // ==========================

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        // ==========================
        // Users
        // ==========================

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("users/{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _adminService.ActivateUserAsync(id);

            if (!result)
                return NotFound("User not found.");

            return Ok(new
            {
                Message = "User activated successfully."
            });
        }

        [HttpPut("users/{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _adminService.DeactivateUserAsync(id);

            if (!result)
                return NotFound("User not found.");

            return Ok(new
            {
                Message = "User deactivated successfully."
            });
        }

        // ==========================
        // Organizations
        // ==========================

        [HttpGet("organizations")]
        public async Task<IActionResult> GetOrganizations()
        {
            var organizations = await _adminService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }
    }
}