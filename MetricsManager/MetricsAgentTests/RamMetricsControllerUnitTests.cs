using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsAgentController controller;

        public RamMetricsControllerUnitTests()
        {
            controller = new RamMetricsAgentController();
        }

        [Fact]
        public void GetMetrics_ReturnsOk()
        {
            var result = controller.GetAvailable();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
