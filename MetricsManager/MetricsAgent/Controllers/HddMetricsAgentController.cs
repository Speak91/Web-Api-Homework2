using AutoMapper;
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
        private readonly IMapper _mapper;
        public HddMetricsAgentController(ILogger<HddMetricsAgentController> logger, IHddMetricsRepository hddMetricsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsAgentController");
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
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все hdd метрику");
            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _hddMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики cpu метрику за определенный период");
            return Ok(response);
        }
    }
}
