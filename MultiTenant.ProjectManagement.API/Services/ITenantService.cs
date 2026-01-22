using MultiTenant.ProjectManagement.API.DTOs.Tenants;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface ITenantService
    {
        Task<List<TenantMemberResponse>> GetTenantMembersAsync();
         Task ChangeMemberRoleAsync(Guid userId, string newRole);
    }
}
