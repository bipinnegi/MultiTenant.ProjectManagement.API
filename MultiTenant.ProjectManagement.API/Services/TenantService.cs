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

       
        // GET: Members of current tenant
      
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

   
        // PATCH: Change member role (Owner only)
       
        public async Task ChangeMemberRoleAsync(Guid userId, string newRole)
        {
            var tenantId = _tenantContext.GetTenantId();
            var currentUserEmail = _tenantContext.GetUserEmail();

            // restrict allowed roles
            var allowedRoles = new[] { "Owner", "Member" };
            if (!allowedRoles.Contains(newRole))
                throw new ArgumentException("Invalid role.");

            // get current user (must be Owner)
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == currentUserEmail &&
                    u.TenantId == tenantId);

            if (currentUser == null || currentUser.Role != "Owner")
                throw new UnauthorizedAccessException("Only owners can change roles.");

            // prevent changing own role
            if (currentUser.Id == userId)
                throw new InvalidOperationException("You cannot change your own role.");

            // fetch target user in same tenant
            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == userId &&
                    u.TenantId == tenantId);

            if (targetUser == null)
                throw new Exception("User not found in this tenant.");

            //prevent removing last Owner
            if (targetUser.Role == "Owner" && newRole == "Member")
            {
                var ownerCount = await _context.Users
                    .CountAsync(u =>
                        u.TenantId == tenantId &&
                        u.Role == "Owner");

                if (ownerCount <= 1)
                    throw new InvalidOperationException(
                        "At least one owner is required."
                    );
            }

            // Apply role change
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

        // Remove member from tenant (only owner can )
        
        public async Task RemoveMemberAsync(Guid userId)
        {
            var tenantId = _tenantContext.GetTenantId();
            var currentUserEmail = _tenantContext.GetUserEmail();

            //get current user (must be Owner)
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == currentUserEmail &&
                    u.TenantId == tenantId);

            if (currentUser == null || currentUser.Role != "Owner")
                throw new UnauthorizedAccessException(
                    "Only owners can remove members."
                );

            // prevent removing self
            if (currentUser.Id == userId)
                throw new InvalidOperationException(
                    "You cannot remove yourself from the tenant."
                );

            // get target user
            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == userId &&
                    u.TenantId == tenantId);

            if (targetUser == null)
                throw new Exception("User not found in this tenant.");

            // Owner removing another owner is not allowed
            if (targetUser.Role == "Owner")
                throw new InvalidOperationException("You cannot remove another owner.");

            _context.Users.Remove(targetUser);

            // Activity log
            _context.ActivityLogs.Add(new ActivityLog
            {
                TenantId = tenantId,
                ActionType = "RemoveMember",
                Message = $"Removed member {targetUser.Email}"
            });

            await _context.SaveChangesAsync();
        }
    }
}
