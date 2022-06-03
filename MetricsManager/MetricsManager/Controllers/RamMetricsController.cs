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
    [Route("api/metrics/ram")]
    [ApiController]
    [SwaggerTag("Менеджер метрик оперативной памяти")]
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

        /// <summary>
        /// Метрики оперативной памяти от определенного агента, за указанный период
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики оперативной памяти от определенного агента, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("getRamMetricsFromAgent")]
        [ProducesResponseType(typeof(RamMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetAvailableFromAgent([FromBody] RamMetricsRequest request)
        {
            RamMetricsResponse response = _metricsAgentClient.GetAllMetrics(request);
            _logger.LogInformation("Информация о метриках процессора от агента передана");
            return Ok(response);
        }

        /// <summary>
        /// Метрики оперативной памяти от всех зарегистрированных агентов, за указанный период
        /// </summary>
        /// <param name="fromTime">Начальная дата</param>
        /// <param name="toTime">Конечная дата</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Метрики оперативной памяти от всех зарегистрированных агентов, за указанный период")]
        [SwaggerResponse(200, "успешная операция")]
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
        }
    }
}
