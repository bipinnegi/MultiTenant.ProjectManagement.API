using System;

namespace MultiTenant.ProjectManagement.API.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }  // Owner / Member

        public Guid TenantId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsAccepted { get; set; }
    }
}
