using System.ComponentModel.DataAnnotations;

namespace MultiTenant.ProjectManagement.API.DTOs.Tenants
{
    public class UpdateMemberRoleRequest
    {
        [Required]
        public string Role {  get; set; }
    }
}
