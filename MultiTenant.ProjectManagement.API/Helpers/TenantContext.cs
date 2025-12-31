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
    }
}
