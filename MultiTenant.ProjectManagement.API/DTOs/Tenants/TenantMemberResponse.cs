namespace MultiTenant.ProjectManagement.API.DTOs.Tenants
{
    public class TenantMemberResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
