using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsAgentController _controller;
        private Mock<ICpuMetricsRepository> mock;
        private Mock<ILogger<CpuMetricsAgentController>> mockLogger;
        private Mock<IMapper> mockMapper;

        public CpuMetricsControllerUnitTests()
        {
            mock = new Mock<ICpuMetricsRepository>();
            mockLogger = new Mock<ILogger<CpuMetricsAgentController>>();
            mockMapper = new Mock<IMapper>();
            _controller = new CpuMetricsAgentController(mockLogger.Object, mock.Object, mockMapper.Object);
        }


        [Fact]
        public void GetAll_ShouldCall_GetAll_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<CpuMetric>());
            var result = _controller.GetAll();
            mock.Verify(repository => repository.GetAll());


        }

        [Fact]
        public void GetMetrics_ShouldCall_GetMetrics_From_Repository()
        {
            var fromTime = new TimeSpan(00, 05, 00);
            var toTime = new TimeSpan(00, 10, 00);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Returns(new List<CpuMetric>());
            var result = _controller.GetMetrics(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
