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

        public async Task<Project> DeleteProjectAsync(Guid projectId)
        {

            var tenantId = _tenantContext.GetTenantId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p =>
                    p.Id == projectId &&
                    p.TenantId == tenantId);

            if (project == null)
            {
                throw new Exception("Project not found or access denied");
            }

            //Delete related tasks first
            var tasks = await _context.TaskItems
                .Where(t =>
                    t.ProjectId == projectId &&
                    t.TenantId == tenantId)
                .ToListAsync();

            _context.TaskItems.RemoveRange(tasks);

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return project;
        }
    }
}
