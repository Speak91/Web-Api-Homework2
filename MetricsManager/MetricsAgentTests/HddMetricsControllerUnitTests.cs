using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsAgentController _controller;
        private Mock<IHddMetricsRepository> mock;
        private Mock<ILogger<HddMetricsAgentController>> mockLogger;
        private Mock<IMapper> mockMapper;
        public HddMetricsControllerUnitTests()
        {
            mock = new Mock<IHddMetricsRepository>();
            mockLogger = new Mock<ILogger<HddMetricsAgentController>>();
            mockMapper = new Mock<IMapper>();
            _controller = new HddMetricsAgentController(mockLogger.Object, mock.Object, mockMapper.Object);
        }

        [Fact]
        public void GetFreeHDDSpace_ShouldCall_GetFreeHDDSpace_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<HddMetric>());
            var result = _controller.GetFreeHDDSpace();
            mock.Verify(repository => repository.GetAll());
        }

        [Fact]
        public void GetMetrics_ShouldCall_GetMetrics_From_Repository()
        {
            var fromTime = new TimeSpan(00, 05, 00);
            var toTime = new TimeSpan(00, 10, 00);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Returns(new List<HddMetric>());
            var result = _controller.GetMetrics(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
