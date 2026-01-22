using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> ChangeMemberRole(Guid userId, [FromBody] UpdateMemberRoleRequest request)
        {
            await _tenantService.ChangeMemberRoleAsync(userId, request.Role);
            return NoContent();
        }

    }
}
