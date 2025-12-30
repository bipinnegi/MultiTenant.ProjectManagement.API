using System.ComponentModel.DataAnnotations;

namespace MultiTenant.ProjectManagement.API.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; }
    }
}
