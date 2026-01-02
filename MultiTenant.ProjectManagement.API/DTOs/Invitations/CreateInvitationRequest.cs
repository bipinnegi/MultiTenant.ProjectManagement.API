namespace MultiTenant.ProjectManagement.API.DTOs.Invitations
{
    public class CreateInvitationRequest
    {
        public string Email { get; set; }
        public string Role { get; set; } // Owner/Member
    }
}
