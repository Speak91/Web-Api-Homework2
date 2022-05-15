using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsAgentController controller;

        public CpuMetricsControllerUnitTests()
        {
            controller = new CpuMetricsAgentController();
        }

        [Fact]
        public void GetMetricsPercentile_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var percentile = Percentile.P99;

            var result = controller.GetMetrics(fromTime, toTime, percentile);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
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
