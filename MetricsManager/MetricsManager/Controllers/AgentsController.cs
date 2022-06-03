using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Предоставляет работу с агентами")]
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
        
        /// <summary>
        /// "Регистрация нового агента в системе мониторинга"
        /// </summary>
        /// <param name="agentUrl">Адрес агента</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Регистрация нового агента в системе мониторинга")]
        [SwaggerResponse(200, "успешная регистрация агента в системе")]
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

        /// <summary>
        /// Включить агента
        /// </summary>
        /// <param name="agentId">Id Агента</param>
        /// <returns></returns>
        [HttpPut("enable/{agentId}")]
        [SwaggerOperation(description: "включение агента")]
        [SwaggerResponse(200, "успешная операция")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            //if (_agentPool.Values.ContainsKey(agentId))
            //    _agentPool.Values[agentId].Enable = true;
            _logger.LogInformation($"Агент {agentId} включен");
            return Ok();
        }

        /// <summary>
        /// Отключение агента
        /// </summary>
        /// <param name="agentId">Id агента</param>
        /// <returns></returns>
        [SwaggerOperation(description: "отключение агента")]
        [SwaggerResponse(200, "успешная операция")]
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            //if (_agentPool.Values.ContainsKey(agentId))
            //    _agentPool.Values[agentId].Enable = false;
            _logger.LogInformation($"Агент {agentId} отключен");
            return Ok();
        }

        /// <summary>
        /// Удаление агента
        /// </summary>
        /// <param name="agentId">Id агента</param>
        /// <returns></returns>
        [SwaggerOperation(description: "Удаление агента")]
        [SwaggerResponse(200, "успешное удаление")]
        [HttpPut("delete/{agentId}")]
        public IActionResult DeleteAgentById([FromRoute] int agentId)
        {
            _agentsRepository.Delete(agentId);
            _logger.LogInformation($"Агент {agentId} удален из базы");
            return Ok();
        }

        /// <summary>
        /// Вернуть список всех агентов
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(description: "Вернуть список всех агента")]
        [SwaggerResponse(200, "успешно возвращен список агентов")]
        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            return Ok(_agentsRepository.Get());
        }
    }
}
