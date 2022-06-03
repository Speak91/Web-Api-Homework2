using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.DTO;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Requests.Response;
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
    [Route("api/metrics/ram")]
    [ApiController]
    [SwaggerTag("Агент метрик оперативной памяти")]
    public class RamMetricsAgentController : ControllerBase
    {
        private IRamMetricsRepository _ramMetricsRepository;
        private readonly ILogger<RamMetricsAgentController> _logger;
        private readonly IMapper _mapper;
        public RamMetricsAgentController(ILogger<RamMetricsAgentController> logger, IRamMetricsRepository ramMetricsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ramMetricsRepository = ramMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsAgentController");
        }

        /// <summary>
        /// Сбор метрик оперативной памяти
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Сбор метрик оперативной памяти")]
        [SwaggerResponse(200, "успешная операция")]
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
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики оперативной памяти");
            return Ok(response);
        }

        /// <summary>
        /// Сбор метрик оперативной памяти за определенный промежуток времени
        /// </summary>
        /// <param name="fromTime">Начальное время</param>
        /// <param name="toTime">Конечное время</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Сбор метрик оперативной памяти за определенный промежуток времени")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _ramMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики cpu метрику за определенный период");
            return Ok(response);
        }
    }
}
