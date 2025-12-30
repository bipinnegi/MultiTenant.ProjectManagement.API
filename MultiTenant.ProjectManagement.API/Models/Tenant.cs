using System.ComponentModel.DataAnnotations;

namespace MultiTenant.ProjectManagement.API.Models
{
    public class Tenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
