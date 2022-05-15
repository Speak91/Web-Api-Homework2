using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;

        public HddMetricsControllerUnitTests()
        {
            controller = new HddMetricsController();
        }

        [Fact]
        public void GetFreeHDDSpaceFromAgent_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.GetFreeHDDSpaceFromAgent(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetFreeHDDSpaceFromAllCluster_ReturnsOk()
        {
            var result = controller.GetFreeHDDSpaceFromAllCluster();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
