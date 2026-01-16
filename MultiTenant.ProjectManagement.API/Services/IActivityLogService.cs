using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface IActivityLogService
    {
        Task LogAsync(
            string actionType,
            string entityType,
            Guid entityId,
            string message
        );

        Task<List<ActivityLog>> GetRecentAsync(int limit = 10);
    }
}
