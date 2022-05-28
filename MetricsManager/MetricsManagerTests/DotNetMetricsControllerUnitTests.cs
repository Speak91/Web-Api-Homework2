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
    public class DotNetMetricsControllerUnitTests
    {
        private DotNetMetricsController controller;
        private Mock<ILogger<DotNetMetricsController>> mockLogger;
        private Mock<IDotNetMetricsAgentClient> _mock;
        private Mock<IHttpClientFactory> _mockHttp;
        public DotNetMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            controller = new DotNetMetricsController(mockLogger.Object, _mock.Object, _mockHttp.Object);
        }

        [Fact]
        public void GetErrorsCountFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = new TimeSpan();
            var toTime = new TimeSpan();

            var result = controller.GetErrorsCountFromAgent(agentId, fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void GetErrorsCountFromAllCluster_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetErrorsCountFromAllCluster(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
