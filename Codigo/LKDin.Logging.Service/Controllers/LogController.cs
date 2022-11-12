using LKDin.Logging.Service.Internal.DTOs;
using LKDin.Logging.Service.Internal.Logging;
using Microsoft.AspNetCore.Mvc;

namespace LKDin.Logging.Service.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly LoggingService _logService;

        public LogController()
        {
            _logService = LoggingService.Instance;
        }

        [HttpGet]
        public IEnumerable<Log> Get([FromQuery] FilterParams parameters)
        {
            return _logService.GetLogs(parameters);
        }
    }
}