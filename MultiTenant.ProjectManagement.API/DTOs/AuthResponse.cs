namespace MultiTenant.ProjectManagement.API.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid TenantId { get; set; }
        public string Role { get; set; }

    }
}
