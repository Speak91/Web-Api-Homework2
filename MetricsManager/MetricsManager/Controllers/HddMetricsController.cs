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
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    [SwaggerTag("Менеджер метрик локального диска")]
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

        /// <summary>
        /// "Метрики локального диска от определенного агента, за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики локального диска от определенного агента, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("getHddMetricsFromAgent")]
        [ProducesResponseType(typeof(HddMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetFreeHDDSpaceFromAgent([FromBody] HddMetricsRequest request)
        {
            HddMetricsResponse response = _metricsAgentClient.GetAllMetrics(request);
            _logger.LogInformation($"Список свободного места на {request.AgentId} передан");
            return Ok(response);
        }

        /// <summary>
        /// Метрики локального диска от всех зарег-ых агентов, за указанный период
        /// </summary>
        /// <param name="fromTime">начальная дата</param>
        /// <param name="toTime">конечная дата</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики локального диска от всех зарег-ых агентов, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
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
