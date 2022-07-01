using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    [SwaggerTag("Менеджер метрик .NET")]
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

        /// <summary>
        /// "Метрики .NET определенного агента, за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики .NET определенного агента, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("errors-countFromAgent")]
        [ProducesResponseType(typeof(DotNetMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetErrorsCountFromAgent([FromBody] DotNetMetricsRequest request)
        {
            DotNetMetricsResponse response = _metricsAgentClient.GetAllMetrics(request);
            _logger.LogInformation($"Список ошибок передан от {request.AgentId}");
            return Ok(response);
        }

        /// <summary>
        /// Метрики .NET от всех зарегистрированных агентов, за указанный период
        /// </summary>
        /// <param name="fromTime">начальная дата</param>
        /// <param name="toTime">конечная дата</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики .NET от всех зарегистрированных агентов, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
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
