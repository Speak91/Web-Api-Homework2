using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    [SwaggerTag("Менеджер метрик сети")]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly INetworkMetricsAgentClient _metricsAgentClient;
        public NetworkMetricsController(ILogger<NetworkMetricsController> logger,
            INetworkMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в NetworkMetricsController");
        }

        /// <summary>
        /// Метрики сети от определенного агента, за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики сети от определенного агента, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("getNetworkMetricsFromAgent")]
        [ProducesResponseType(typeof(NetworkMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetMetricsFromAgent([FromBody] NetworkMetricsRequest request)
        {
            NetworkMetricsResponse response = _metricsAgentClient.GetAllMetrics(request);
            _logger.LogInformation($"Метрика сети от агента {request.AgentId} передана");
            return Ok(response);
        }

        /// <summary>
        /// Метрики сети от всех зарегистрированных агентов, за указанный период
        /// </summary>
        /// <param name="fromTime">Начальная дата</param>
        /// <param name="toTime">Конечная дата</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики сети от всех зарегистрированных агентов, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            List<NetworkMetricsResponse> metrics = _metricsAgentClient.GetMetricsNetworkFromAllCluster(new NetworkMetricsRequest()
            {
                FromTime = fromTime,
                ToTime = toTime
            });
            _logger.LogInformation($"Метрика сети от кластера передана");
            return Ok(metrics);
        }
    }
}
