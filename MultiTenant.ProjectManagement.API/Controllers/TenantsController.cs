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

        // centralised allowed roles validation
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

      
        // GET: List all members of current tenant
       
        [HttpGet("members")]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _tenantService.GetTenantMembersAsync();
            return Ok(members); // 200
        }

      
        // PATCH: Change member role (Owner only)
      
        [HttpPatch("members/{userId}/role")]
        public async Task<IActionResult> ChangeMemberRole(
            Guid userId,
            [FromBody] UpdateMemberRoleRequest request)
        {
            // validate request body
            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest("Role is required."); // 400
            }

            var newRole = request.Role.Trim();

            // validate allowed roles
            if (!AllowedRoles.Contains(newRole))
            {
                return BadRequest(
                    "Invalid role. Allowed roles: Owner, Member."
                ); // 400
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


        // DELETE: Remove member from tenant (Owner only)
        [HttpDelete("members/{userId}")]
        public async Task<IActionResult> RemoveMember(Guid userId)
        {
            try
            {
                await _tenantService.RemoveMemberAsync(userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
