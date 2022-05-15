using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkMetricsControllerUnitTests
    {
        private NetworkMetricsAgentController controller;

        public NetworkMetricsControllerUnitTests()
        {
            controller = new NetworkMetricsAgentController();
        }

        [Fact]
        public void GetMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetrics(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
