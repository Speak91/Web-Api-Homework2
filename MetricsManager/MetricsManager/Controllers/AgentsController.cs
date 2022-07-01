using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private IAgentsRepository _agentsRepository;
        public AgentsController(ILogger<AgentsController> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в AgentsController");
        }

        [HttpPut("register/{agentUrl}")]
        public IActionResult RegisterAgent([FromRoute] string agentUrl)
        {
            if (agentUrl != null)
            {
                _agentsRepository.Create(HttpUtility.UrlDecode(agentUrl));
                _logger.LogInformation("Агент зарегистрирован");
            }
            return Ok(agentUrl.ToString());
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            //if (_agentPool.Values.ContainsKey(agentId))
            //    _agentPool.Values[agentId].Enable = true;
            _logger.LogInformation($"Агент {agentId} включен");
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            //if (_agentPool.Values.ContainsKey(agentId))
            //    _agentPool.Values[agentId].Enable = false;
            _logger.LogInformation($"Агент {agentId} отключен");
            return Ok();
        }

        [HttpPut("delete/{agentId}")]
        public IActionResult DeleteAgentById([FromRoute] int agentId)
        {
            _agentsRepository.Delete(agentId);
            _logger.LogInformation($"Агент {agentId} удален из базы");
            return Ok();
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            return Ok(_agentsRepository.Get());
        }
    }
}
