using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsManager.Services.Impl;
using System.Net.Http;
using MetricsManager.Models.Request;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHddMetricsAgentClient _metricsAgentClient;

        public HddMetricsController(ILogger<HddMetricsController> logger,
            IHddMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }

        [HttpGet("left/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetFreeHDDSpaceFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            HddMetricsResponse response = _metricsAgentClient.GetAllMetrics(new HddMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation($"Список свободного места на {agentId} передан");
            return Ok(response);
        }

        [HttpGet("left/cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetFreeHDDSpaceFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            List<HddMetricsResponse> metrics = _metricsAgentClient.GetMetricsHddFromAllCluster(new HddMetricsRequest()
            {
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation("Список свободного места на кластере передан");
            return Ok(metrics);
        }
    }
}
