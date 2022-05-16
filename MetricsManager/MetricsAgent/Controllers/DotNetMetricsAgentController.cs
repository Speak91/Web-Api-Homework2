using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
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
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsAgentController : ControllerBase
    {
        private IDotNetMetricsRepository _dotNetMetricsRepository;
        private readonly ILogger<DotNetMetricsAgentController> _logger;
        
        public DotNetMetricsAgentController(
            ILogger<DotNetMetricsAgentController> logger,
            IDotNetMetricsRepository dotNetMetricsRepository)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            DotNetMetric dotNetMetric = new DotNetMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _dotNetMetricsRepository.Create(dotNetMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую dotNet метрику: {0}", dotNetMetric);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _dotNetMetricsRepository.GetAll();
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики cpu метрику");
            return Ok(response);
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetErrorsCount([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно передали все метрики за определенный период");
            return Ok();
        }

    }
}
