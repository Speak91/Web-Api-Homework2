using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Http;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    [SwaggerTag("Менеджер метрик процессора")]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICpuMetricsAgentClient _metricsAgentClient;

        public CpuMetricsController(ILogger<CpuMetricsController> logger,
            ICpuMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        /// <summary>
        /// Показать метрики процессора определенного агента за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики процессора определенного агента, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("getCpuMetricsFromAgent")]
        [ProducesResponseType(typeof(CpuMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetMetricsFromAgent([FromBody] CpuMetricsRequest request)
        {
            CpuMetricsResponse response = _metricsAgentClient.GetAllMetrics(request);
            _logger.LogInformation("Информация о метриках процессора от агента передана");
            return Ok(response);
        }

        /// <summary>
        /// Показать метрики процессора от всех зарегистрированных агентов, за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики процессора от всех зарегистрированных агентов, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("getMetricsFromCpuCluster")]
        public IActionResult GetMetricsFromAllCluster([FromBody] CpuMetricsRequest request)
        {
            List<CpuMetricsResponse> metrics = _metricsAgentClient.GetMetricsCpuFromAllCluster(new CpuMetricsRequest()
            {
                FromTime = request.FromTime,
                ToTime = request.ToTime
            });
            _logger.LogInformation("Информация о метриках процессора от кластера передана");
            return Ok(metrics);
        }
    }
}
