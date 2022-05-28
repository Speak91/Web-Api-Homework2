using MetricsManager.Controllers;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace MetricsManagerTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<ILogger<RamMetricsController>> mockLogger;
        private Mock<IRamMetricsAgentClient> _mock;
        private Mock<IHttpClientFactory> _mockHttp;

        public RamMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<RamMetricsController>>();
            controller = new RamMetricsController(mockLogger.Object, _mock.Object, _mockHttp.Object);
        }

        [Fact]
        public void GetAvailableFromAgent_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.GetAvailableFromAgent(agentId, new TimeSpan(), new TimeSpan(12,1,5));

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAvailableFromAllCluster_ReturnsOk()
        {
            var result = controller.GetAvailableFromAllCluster(new TimeSpan(), new TimeSpan(12, 1, 5));

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
