using System.ComponentModel.DataAnnotations;

namespace MultiTenant.ProjectManagement.API.DTOs
{
    public class RegisterTenantRequest
    {
        [Required]
        public string TenantName { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required, EmailAddress]
        public string OwnerEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
