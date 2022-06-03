using AutoMapper;
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
        private readonly IMapper _mapper;
        public DotNetMetricsAgentController(
            ILogger<DotNetMetricsAgentController> logger,
            IDotNetMetricsRepository dotNetMetricsRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _dotNetMetricsRepository.Create(_mapper.Map<DotNetMetric>(request));

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую dotNet метрику: {0}", request);

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
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
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
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно передали все метрики за определенный период");
            return Ok(response);
        }

    }
}
