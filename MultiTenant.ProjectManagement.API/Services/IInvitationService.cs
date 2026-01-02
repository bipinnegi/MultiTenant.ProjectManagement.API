using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface IInvitationService
    {
        Task<Invitation> CreateInvitationAsync(string email, string role);

        Task AcceptInvitationAsync(string token, string fullName, string password);

    }
}
