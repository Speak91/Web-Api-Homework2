using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDotNetMetricsAgentClient _metricsAgentClient;

        public DotNetMetricsController(ILogger<DotNetMetricsController> logger,
             IDotNetMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
        }

        [HttpGet("errors-count/agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetErrorsCountFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            DotNetMetricsResponse response = _metricsAgentClient.GetAllMetrics(new DotNetMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation($"Список ошибок передан от {agentId}");
            return Ok(response);
        }
        [HttpGet("errors-count/cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetErrorsCountFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            List<DotNetMetricsResponse> metrics = _metricsAgentClient.GetMetricsDotNetFromAllCluster(new DotNetMetricsRequest()
            {
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation("Список ошибок передан от кластера");
            return Ok(metrics);
        }
    }
}
