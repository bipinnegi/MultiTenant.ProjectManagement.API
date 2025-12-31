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
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "tenantId");
            if (tenantIdClaim != null)
            {
                throw new Exception("TenantId not found in token");
            }
            return Guid.Parse(tenantIdClaim.Value);
        }
    }
}
