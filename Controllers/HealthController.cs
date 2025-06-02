using System;
using Microsoft.AspNetCore.Mvc;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Status = "OK", Timestamp = DateTime.UtcNow });
        }
    }
}