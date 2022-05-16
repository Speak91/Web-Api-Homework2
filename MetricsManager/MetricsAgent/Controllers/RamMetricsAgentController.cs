using MetricsAgent.Models;
using MetricsAgent.Models.DTO;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Response;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsAgentController : ControllerBase
    {
        private IRamMetricsRepository _ramMetricsRepository;
        private readonly ILogger<RamMetricsAgentController> _logger;
        public RamMetricsAgentController(ILogger<RamMetricsAgentController> logger, IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            RamMetric networkMetric = new RamMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _ramMetricsRepository.Create(networkMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую метрику оперативной памяти: {0}", networkMetric);

            return Ok();
        }

        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            var metrics = _ramMetricsRepository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики оперативной памяти");
            return Ok(response);
        }
    }
}
