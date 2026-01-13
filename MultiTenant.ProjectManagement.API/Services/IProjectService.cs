using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(string  name, string discription );
        Task<List<Project>> GetAllAsync();

        Task<Project> DeleteProjectAsync(Guid projectId);

    }
}
