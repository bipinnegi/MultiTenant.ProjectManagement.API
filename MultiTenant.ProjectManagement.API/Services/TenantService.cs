using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.DTOs.Tenants;
using MultiTenant.ProjectManagement.API.Helpers;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class TenantService: ITenantService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;

        public TenantService(AppDbContext context, TenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }

        public async Task<List<TenantMemberResponse>> GetTenantMembersAsync()
        {
            var tenantId = _tenantContext.GetTenantId();
            return await _context.Users.Where(u => u.TenantId == tenantId)
                                       .Select(u => new TenantMemberResponse
                                       {
                                           Id = u.Id,
                                           FullName = u.FullName,
                                           Email = u.Email,
                                           Role = u.Role
                                       }).OrderBy(u => u.FullName)
                                         .ToListAsync();
        }
    }
}
    