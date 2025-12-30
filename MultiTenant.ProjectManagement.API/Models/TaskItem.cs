using System.ComponentModel.DataAnnotations;
namespace MultiTenant.ProjectManagement.API.Models
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Status { get; set; } // Todo, InProgress, Done

        [Required]
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        [Required]
        public Guid TenantId { get; set; }
    }
}
