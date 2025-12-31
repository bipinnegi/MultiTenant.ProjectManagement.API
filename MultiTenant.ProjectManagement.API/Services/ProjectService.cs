using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.Helpers;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class ProjectService: IProjectService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;

        public ProjectService(AppDbContext context, TenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }

        public async Task<Project> CreateAsync(string name, string description)
        {
            //TenantId comes from JWT
            var tenantId = _tenantContext.GetTenantId();

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                TenantId = tenantId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var tenantId = _tenantContext.GetTenantId();

            return await _context.Projects.Where(p => p.TenantId == tenantId).ToListAsync();
        }
    }
}
