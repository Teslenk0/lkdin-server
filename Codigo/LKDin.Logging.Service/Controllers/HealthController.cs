using Microsoft.AspNetCore.Mvc;

namespace LKDin.Logging.Service.Controllers
{
    [Route("_health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            return new { Application = "LKDin.Logging.Service", Healthy = true, };
        }
    }
}
