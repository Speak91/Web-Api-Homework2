using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<ILogger<RamMetricsController>> mockLogger;

        public RamMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<RamMetricsController>>();
            controller = new RamMetricsController(mockLogger.Object);
        }

        [Fact]
        public void GetAvailableFromAgent_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.GetAvailableFromAgent(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAvailableFromAllCluster_ReturnsOk()
        {
            var result = controller.GetAvailableFromAllCluster();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
