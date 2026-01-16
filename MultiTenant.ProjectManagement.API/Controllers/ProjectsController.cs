using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.DTOs;
using MultiTenant.ProjectManagement.API.Services;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IActivityLogService _activityLogService;
        public ProjectsController(IProjectService projectService, IActivityLogService  activityLogService) 
        { 
         _projectService = projectService;
            _activityLogService = activityLogService;
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(CreateProjectRequest request)
        {
            var project = await _projectService.CreateAsync(
                request.Name,
                request.Description
            );
            await _activityLogService.LogAsync(
                actionType: "CREATE",
                entityType: "Project",
                entityId: project.Id,
                message: $"Project '{project.Name}' was created"
            );

            return Ok(project);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }



        [HttpDelete("{projectId}")]
        [Authorize(Roles ="Owner")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var project = await _projectService.DeleteProjectAsync(projectId);
            await _activityLogService.LogAsync(
               actionType: "DELETE",
               entityType: "Project",
               entityId: project.Id,
               message: $"Project '{project.Name}' was deleted"
            );
            return Ok(project);
        }



    }
    
}
