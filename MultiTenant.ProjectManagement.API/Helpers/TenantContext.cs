using MultiTenant.ProjectManagement.API.Models;
using System.Security.Claims;

namespace MultiTenant.ProjectManagement.API.Helpers
{
    public class TenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext?.User
            ?? throw new Exception("HttpContext not available");

        public Guid GetTenantId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                throw new Exception("User is not authenticated");
            }

            //SAFEST way to read custom claim
            var tenantIdValue = user.FindFirstValue("tenantId");

            if (string.IsNullOrEmpty(tenantIdValue))
            {
                throw new Exception("TenantId not found in token");
            }

            return Guid.Parse(tenantIdValue);
        }

        public Guid GetUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userIdValue))
                throw new Exception("UserId not found in token");

            return Guid.Parse(userIdValue);
        }

        public string GetUserRole()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(role))
                throw new Exception("Role not found in token");

            return role;
        }
    }
}
