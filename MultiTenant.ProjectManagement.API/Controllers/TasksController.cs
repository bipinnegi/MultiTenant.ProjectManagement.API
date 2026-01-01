using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.Services;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService) 
        { 
          _taskService = taskService;
        }

        [HttpPost] // POST: /api/projects/{projectId}/tasks
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateTask(Guid projectId, CreateTaskRequest request)
        {
            var task = await _taskService.CreateTaskAsync(projectId, request.Title);
            return Ok(task);
        }

        [HttpGet]  // GET: /api/projects/{projectId}/tasks
        public async Task<IActionResult> GetTasks(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectAsync(projectId);
            return Ok(tasks);
        }

        [HttpPatch("{taskId}/status")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateStatus(Guid projectId, Guid taskId, UpdateTaskStatusRequest request)
        {
            var task = await _taskService.UpdateTaskStatusAsync(projectId, taskId, request.Status);
            return Ok(task);
        }


    }
    public class CreateTaskRequest
    {
        public string Title { get; set; }
    }

    public class UpdateTaskStatusRequest
    {
        public string Status { get; set; }
    }

}
