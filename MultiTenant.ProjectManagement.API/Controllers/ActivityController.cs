using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.ProjectManagement.API.Services;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    
    [ApiController]
    [Route("api/activity")]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        public ActivityController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecenet()
        {
            var activity = await _activityLogService.GetRecentAsync();
            return Ok(activity);
        }
    }
}
