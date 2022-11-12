using Microsoft.AspNetCore.Mvc;

namespace LKDin.Admin.Controllers
{
    [Route("_health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public Object Get()
        {
            return new { Application = "LKDin.Logging.Service", Healthy = true, };
        }
    }
}
