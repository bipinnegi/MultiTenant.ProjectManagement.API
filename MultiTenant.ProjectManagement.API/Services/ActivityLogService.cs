using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.Helpers;
using MultiTenant.ProjectManagement.API.Models;
using System.Security.Claims;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLogService(
            AppDbContext context,
            TenantContext tenantContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tenantContext = tenantContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string actionType, string entityType, Guid entityId, string message)
        {
            var tenantId = _tenantContext.GetTenantId();

            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("User not authenticated");

            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                UserId = Guid.Parse(userIdClaim.Value),
                ActionType = actionType,
                EntityType = entityType,
                EntityId = entityId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivityLog>> GetRecentAsync(int limit = 10)
        {
            var tenantId = _tenantContext.GetTenantId();

            return await _context.ActivityLogs
                .Where(a => a.TenantId == tenantId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}
