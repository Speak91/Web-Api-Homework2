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
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsAgentController _controller;
        private Mock<IRamMetricsRepository> mock;
        private Mock<ILogger<RamMetricsAgentController>> mockLogger;
        private Mock<IMapper> mockMapper;
        public RamMetricsControllerUnitTests()
        {
            mock = new Mock<IRamMetricsRepository>();
            mockLogger = new Mock<ILogger<RamMetricsAgentController>>();
            mockMapper = new Mock<IMapper>();
            _controller = new RamMetricsAgentController(mockLogger.Object, mock.Object, mockMapper.Object);
        }

        [Fact]
        public void GetAvailable_ShouldCall_GetAvailable_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<RamMetric>());
            var result = _controller.GetAvailable();
            mock.Verify(repository => repository.GetAll());
        }

        [Fact]
        public void GetMetrics_ShouldCall_GetMetrics_From_Repository()
        {
            var fromTime = new TimeSpan(00, 05, 00);
            var toTime = new TimeSpan(00, 10, 00);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Returns(new List<RamMetric>());
            var result = _controller.GetMetrics(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
