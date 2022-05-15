using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class DotNetMetricsControllerUnitTests
    {
        private DotNetMetricsAgentController controller;

        public DotNetMetricsControllerUnitTests()
        {
            controller = new DotNetMetricsAgentController();
        }

        [Fact]
        public void GetErrorsCount_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetErrorsCount(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
