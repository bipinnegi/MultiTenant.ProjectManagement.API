using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.DTOs.Tasks;
using MultiTenant.ProjectManagement.API.Services;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IActivityLogService _activityLogService;
        public TasksController(ITaskService taskService, IActivityLogService activityLogService) 
        { 
          _taskService = taskService;
            _activityLogService = activityLogService;
        }

        [HttpPost] // POST: /api/projects/{projectId}/tasks
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateTask(Guid projectId, CreateTaskRequest request)
        {
            var task = await _taskService.CreateTaskAsync(projectId, request.Title);

            await _activityLogService.LogAsync(actionType: "Create",
                                               entityType: "Task",
                                               entityId: task.Id,
                                               message: $"Task \"{task.Title}\"Created"
                                               );

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

            // ACTIVITY LOG
            await _activityLogService.LogAsync(
                actionType: "Update",
                entityType: "Task",
                entityId: task.Id,
                message: $"Task \"{task.Title}\" moved to {task.Status}"
            );

            return Ok(task);
        }

        [HttpDelete("{taskId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var deletedTask = await _taskService.DeleteTaskAsync(projectId, taskId);

            await _activityLogService.LogAsync(
                                               actionType: "Delete",
                                               entityType: "Task",
                                               entityId: deletedTask.Id,
                                               message: $"Task \"{deletedTask.Title}\" deleted"
                                              );
            return Ok(deletedTask);
        }



    }


}
