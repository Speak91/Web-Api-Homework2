using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsAgentController controller;

        public HddMetricsControllerUnitTests()
        {
            controller = new HddMetricsAgentController();
        }

        [Fact]
        public void GetFreeHDDSpace_ReturnsOk()
        {
            var result = controller.GetFreeHDDSpace();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
