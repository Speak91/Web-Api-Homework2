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
    [Route("api/metrics/network")]
    [ApiController]
    [SwaggerTag("Агент метрик сети")]
    public class NetworkMetricsAgentController : ControllerBase
    {
        private INetworkMetricsRepository _networkMetricsRepository;
        private readonly ILogger<NetworkMetricsAgentController> _logger;
        private readonly IMapper _mapper;
        public NetworkMetricsAgentController(ILogger<NetworkMetricsAgentController> logger, INetworkMetricsRepository networkMetricsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _networkMetricsRepository = networkMetricsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в NetworkMetricsAgentController");
        }

        /// <summary>
        /// Сбор метрик сети
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Сбор метрик сети")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _networkMetricsRepository.GetAll();
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики сети");
            return Ok(response);
        }

        /// <summary>
        /// Сбор метрик cети за определенный промежуток времени
        /// </summary>
        /// <param name="fromTime">Начальное время</param>
        /// <param name="toTime">Конечное время</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Сбор метрик cети за определенный промежуток времени")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _networkMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики сети за определенный период");
            return Ok(response);
        }
    }
}
