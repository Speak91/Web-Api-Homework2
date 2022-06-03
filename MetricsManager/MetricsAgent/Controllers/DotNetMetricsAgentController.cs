using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    [SwaggerTag("Агент метрик .NET")]
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

        /// <summary>
        /// Сбор метрик .NET
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [SwaggerOperation(description: "Сбор метрик .NET")]
        [SwaggerResponse(200, "успешная операция")]
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

        /// <summary>
        /// Сбор метрик .NET за определенный промежуток времени
        /// </summary>
        /// <param name="fromTime">Начальное время</param>
        /// <param name="toTime">Конечное время</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Сбор метрик .NET за определенный промежуток времени")]
        [SwaggerResponse(200, "успешная операция")]
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
