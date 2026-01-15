namespace MultiTenant.ProjectManagement.API.Models
{
    public class ActivityLog
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        // Who performed the action
        public Guid UserId { get; set; }

        // What happened
        public string ActionType { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public Guid EntityId { get; set; }

        // Human-readable message for UI
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
