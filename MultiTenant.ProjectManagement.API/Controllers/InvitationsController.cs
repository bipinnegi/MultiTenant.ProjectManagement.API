using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.Services;
using MultiTenant.ProjectManagement.API.DTOs.Invitations;


namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/invitations")]
    public class InvitationsController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationsController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        // OWNER creates an invitation
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateInvitation(CreateInvitationRequest request)
        {
            var invitation = await _invitationService.CreateInvitationAsync(
                request.Email,
                request.Role
            );

            // In real SaaS, token is emailed
            // Here we return it for testing
            return Ok(new
            {
                invitation.Email,
                invitation.Role,
                invitation.Token,
                invitation.ExpiresAt
            });
        }

        // Invitee accepts invitation
        [HttpPost("accept")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvitation(AcceptInvitationRequest request)
        {
            await _invitationService.AcceptInvitationAsync(
                         request.Token,
                        request.FullName,
                           request.Password
              );


            return Ok("Invitation accepted successfully");
        }
    }

}
