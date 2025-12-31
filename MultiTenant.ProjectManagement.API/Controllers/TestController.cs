using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenant.ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok(new{message = "You are authenticated successfully 🎉"});
        }

    }
}
