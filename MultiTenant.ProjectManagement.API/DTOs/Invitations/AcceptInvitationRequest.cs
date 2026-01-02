namespace MultiTenant.ProjectManagement.API.DTOs.Invitations
{
    public class AcceptInvitationRequest
    {
        public string Token { get; set; }
        public string FullName { get; set; }

        public string Password { get; set; }
    }
}
