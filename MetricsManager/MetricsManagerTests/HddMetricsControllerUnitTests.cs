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
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;
        private Mock<ILogger<HddMetricsController>> mockLogger;
        private Mock<IHddMetricsAgentClient> _mock;
        private Mock<IHttpClientFactory> _mockHttp;

        public HddMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            controller = new HddMetricsController(mockLogger.Object, _mock.Object, _mockHttp.Object);
        }

        [Fact]
        public void GetFreeHDDSpaceFromAgent_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.GetFreeHDDSpaceFromAgent(agentId, new TimeSpan(1, 12, 13), new TimeSpan(2, 12, 13));

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetFreeHDDSpaceFromAllCluster_ReturnsOk()
        {
            var result = controller.GetFreeHDDSpaceFromAllCluster(new TimeSpan(1, 12,13), new TimeSpan(2, 12, 13));

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
