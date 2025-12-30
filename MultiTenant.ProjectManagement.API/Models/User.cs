using System.ComponentModel.DataAnnotations;

namespace MultiTenant.ProjectManagement.API.Models
{
    public class User
    {
        [Key]
        public  Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // Owner/Admin/ Member

        [Required]
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; }
    }
}
