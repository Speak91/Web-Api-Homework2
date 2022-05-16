using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsController(ILogger<RamMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
        }
        [HttpGet("available/agent/{agentId}")]
        public IActionResult GetAvailableFromAgent([FromRoute] int agentId)
        {
            _logger.LogInformation($"Метрика по оперативной памяти от агента {agentId} передана");
            return Ok();
        }
        [HttpGet("available/cluster")]
        public IActionResult GetAvailableFromAllCluster()
        {
            _logger.LogInformation($"Метрика по оперативной памяти от кластера передана");
            return Ok();
        }
    }
}
