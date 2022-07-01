using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRamMetricsAgentClient _metricsAgentClient;
       
        public RamMetricsController(ILogger<RamMetricsController> logger,
            IRamMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
        }
        
        [HttpGet("available/agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetAvailableFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            RamMetricsResponse response = _metricsAgentClient.GetAllMetrics(new RamMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation("Информация о метриках процессора от агента передана");
            return Ok(response);
            _logger.LogInformation($"Метрика по оперативной памяти от агента {agentId} передана");
            return Ok();
        }

        [HttpGet("available/cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetAvailableFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            List<RamMetricsResponse> metrics = _metricsAgentClient.GetMetricsRamFromAllCluster(new RamMetricsRequest()
            {
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation("Информация о метриках процессора от кластера передана");
            return Ok(metrics);
            _logger.LogInformation($"Метрика по оперативной памяти от кластера передана");
            return Ok();
        }
    }
}
