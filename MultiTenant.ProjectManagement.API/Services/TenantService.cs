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
            var currentUserEmail = _tenantContext.GetUserEmail();

            // 1️⃣ Validate role input
            var allowedRoles = new[] { "Owner", "Member" };
            if (!allowedRoles.Contains(newRole))
                throw new ArgumentException("Invalid role.");

            // 2️⃣ Get current user
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == currentUserEmail && u.TenantId == tenantId);

            if (currentUser == null || currentUser.Role != "Owner")
                throw new UnauthorizedAccessException("Only owners can change roles.");

            // 3️⃣ Prevent changing own role
            if (currentUser.Id == userId)
                throw new InvalidOperationException("You cannot change your own role.");

            // 4️⃣ Get target user
            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

            if (targetUser == null)
                throw new Exception("User not found in this tenant.");

            // 5️⃣ Prevent removing last owner
            if (targetUser.Role == "Owner" && newRole == "Member")
            {
                var ownerCount = await _context.Users
                    .CountAsync(u => u.TenantId == tenantId && u.Role == "Owner");

                if (ownerCount <= 1)
                    throw new InvalidOperationException("At least one owner is required.");
            }

            // 6️⃣ Apply role change
            targetUser.Role = newRole;

            // 7️⃣ Log activity
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
