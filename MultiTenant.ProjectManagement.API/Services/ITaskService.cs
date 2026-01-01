using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateTaskAsync(Guid projectId, string title );

        Task<List<TaskItem>> GetTasksByProjectAsync( Guid projectId);
        Task<TaskItem> UpdateTaskStatusAsync(Guid projectId, Guid taskId, string status);
    }
}
