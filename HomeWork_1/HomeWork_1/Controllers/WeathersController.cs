using HomeWork_1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork_1.Controllers
{
    [ApiController]
    [Route("api/crud")]
    public class WeathersController : ControllerBase
    {
        private ValuesHolder _values;

        public WeathersController(ValuesHolder values)
        {
            _values = values;
        }

        [HttpPost("add")]
        public IActionResult Add([FromQuery] Weather weather)
        {
            _values.Add(weather);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime date, [FromQuery]int temperature)
        {
            _values.Update(date, temperature);
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime fromTime, [FromQuery]DateTime toTime)
        {
            try
            {
                _values.Delete(fromTime, toTime);
                return Ok();
            }

            catch (Exception)
            {
                return BadRequest();
            }
           
        }

        [HttpGet("get")]
        public IEnumerable<Weather> Get([FromQuery] DateTime fromTime, [FromQuery] DateTime toTime)
        {
            return _values.GetTemperatures(fromTime, toTime);
        }

        [HttpGet("random")]
        public IActionResult Random(int a)
        {
            _values.Random(a);
            return Ok();
        }

    }
}
