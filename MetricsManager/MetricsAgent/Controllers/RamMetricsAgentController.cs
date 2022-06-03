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
    [Route("api/metrics/ram")]
    [ApiController]
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

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _ramMetricsRepository.Create(_mapper.Map<RamMetric>(request));

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую метрику оперативной памяти: {0}", request);

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
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно показали все метрики оперативной памяти");
            return Ok(response);
        }
    }
}
