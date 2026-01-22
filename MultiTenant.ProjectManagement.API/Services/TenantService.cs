using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.DTOs.Tenants;
using MultiTenant.ProjectManagement.API.Helpers;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class TenantService : ITenantService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;

        public TenantService(AppDbContext context, TenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }


        // Get members of current tenant
        
        public async Task<List<TenantMemberResponse>> GetTenantMembersAsync()
        {
            var tenantId = _tenantContext.GetTenantId();

            return await _context.Users
                .Where(u => u.TenantId == tenantId)
                .OrderBy(u => u.FullName)
                .Select(u => new TenantMemberResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();
        }

      
        // Change member role (Owner only)
        public async Task ChangeMemberRoleAsync(Guid userId, string newRole)
        {
            var tenantId = _tenantContext.GetTenantId();
            var currentUserId = _tenantContext.GetUserId();
            var currentUserRole = _tenantContext.GetUserRole();

            // Only Owner can change roles
            if (currentUserRole != "Owner")
                throw new UnauthorizedAccessException("Only owners can change roles.");

            // Cannot change own role
            if (currentUserId == userId)
                throw new InvalidOperationException("You cannot change your own role.");

            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

            if (targetUser == null)
                throw new Exception("User not found in this tenant.");

            // Prevent removing last owner
            if (targetUser.Role == "Owner" && newRole == "Member")
            {
                var ownerCount = await _context.Users
                    .CountAsync(u => u.TenantId == tenantId && u.Role == "Owner");

                if (ownerCount <= 1)
                    throw new InvalidOperationException("At least one owner is required.");
            }

            // Update role
            targetUser.Role = newRole;

            // Activity log
            _context.ActivityLogs.Add(new ActivityLog
            {
                TenantId = tenantId,
                ActionType = "ChangeMemberRole",
                Message = $"Changed role of {targetUser.Email} to {newRole}"
            });

            await _context.SaveChangesAsync();
        }
    }
}
