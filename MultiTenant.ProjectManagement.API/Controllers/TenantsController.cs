using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.DTOs.Tenants;
using MultiTenant.ProjectManagement.API.Services;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    [Authorize]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        
        private static readonly HashSet<string> AllowedRoles =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "Owner",
                "Member"
            };

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("members")]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _tenantService.GetTenantMembersAsync();
            return Ok(members);
        }

        [HttpPatch("members/{userId}/role")]
        public async Task<IActionResult> ChangeMemberRole(
            Guid userId,
            [FromBody] UpdateMemberRoleRequest request)
        {
            //  Validate request body
            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest("Role is required.");
            }

            var newRole = request.Role.Trim();

            //Validate allowed roles
            if (!AllowedRoles.Contains(newRole))
            {
                return BadRequest("Invalid role. Allowed roles: Owner, Member.");
            }

            try
            {
                await _tenantService.ChangeMemberRoleAsync(userId, newRole);
                return NoContent(); // 204
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // 409
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // 404
            }
        }
    }
}
