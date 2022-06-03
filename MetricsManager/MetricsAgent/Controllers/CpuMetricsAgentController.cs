using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.Services;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    [SwaggerTag("Агент метрик процессора")]
    public class CpuMetricsAgentController : ControllerBase
    {
        private ICpuMetricsRepository _cpuMetricsRepository;
        private readonly ILogger<CpuMetricsAgentController> _logger;
        private readonly IMapper _mapper;
        public CpuMetricsAgentController(
            ILogger<CpuMetricsAgentController> logger, 
            ICpuMetricsRepository cpuMetricsRepository, 
            IMapper mapper)
        {
            _mapper = mapper;
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsAgentController");
        }

        [HttpGet("all")]
        [SwaggerOperation(description: "Сбор метрик процессора")]
        [SwaggerResponse(200, "успешная операция")]
        public IActionResult GetAll()
        {
            var metrics = _cpuMetricsRepository.GetAll();
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики cpu метрику");
            return Ok(response);
        }

        [SwaggerOperation(description: "Сбор метрик процессора за определенный промежуток времени")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _cpuMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики cpu метрику за определенный период");
            return Ok(response);
        }
    }
}
