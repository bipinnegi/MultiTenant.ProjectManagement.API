using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.Helpers;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;

        public TaskService(AppDbContext context, TenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }

        public async Task<TaskItem> CreateTaskAsync(Guid projectId, string title)
        {
            var tenantId = _tenantContext.GetTenantId();

            //  SECURITY CHECK: Project must belong to tenant
            var project = await _context.Projects
                .FirstOrDefaultAsync(p =>
                    p.Id == projectId &&
                    p.TenantId == tenantId);

            if (project == null)
            {
                throw new Exception("Project not found or access denied");
            }

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                Status = "Todo",
                ProjectId = projectId,
                TenantId = tenantId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId)
        {
            var tenantId = _tenantContext.GetTenantId();

            // SECURITY CHECK again
            var projectExists = await _context.Projects
                .AnyAsync(p =>
                    p.Id == projectId &&
                    p.TenantId == tenantId);

            if (!projectExists)
            {
                throw new Exception("Project not found or access denied");
            }

            return await _context.TaskItems
                .Where(t =>
                    t.ProjectId == projectId &&
                    t.TenantId == tenantId)
                .ToListAsync();
        }
    }
}
