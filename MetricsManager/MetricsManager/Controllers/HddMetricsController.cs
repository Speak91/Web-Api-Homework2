using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        [HttpGet("left/agent/{agentId}")]
        public IActionResult GetFreeHDDSpaceFromAgent([FromRoute] int agentId)
        {
            return Ok();
        }
        [HttpGet("left/cluster")]
        public IActionResult GetFreeHDDSpaceFromAllCluster()
        {
            return Ok();
        }
    }
}
