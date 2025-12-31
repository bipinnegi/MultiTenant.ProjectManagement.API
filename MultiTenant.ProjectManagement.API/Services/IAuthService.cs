using MultiTenant.ProjectManagement.API.DTOs;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterTenantAsync(RegisterTenantRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
