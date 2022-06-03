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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsAgentController : ControllerBase
    {
        private IHddMetricsRepository _hddMetricsRepository;
        private readonly ILogger<HddMetricsAgentController> _logger;
        public HddMetricsAgentController(ILogger<HddMetricsAgentController> logger, IHddMetricsRepository hddMetricsRepository)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            HddMetric hddMetric = new HddMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _hddMetricsRepository.Create(hddMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую Hdd метрику: {0}", hddMetric);

            return Ok();
        }

        [HttpGet("left")]
        public IActionResult GetFreeHDDSpace()
        {
            var metrics = _hddMetricsRepository.GetAll();
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все hdd метрику");
            return Ok(response);
        }
    }
}
