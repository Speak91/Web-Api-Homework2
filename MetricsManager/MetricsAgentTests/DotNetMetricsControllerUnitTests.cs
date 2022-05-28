using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
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
    public class DotNetMetricsControllerUnitTests
    {
        private DotNetMetricsAgentController _controller;
        private Mock<IDotNetMetricsRepository> mock;
        private Mock<ILogger<DotNetMetricsAgentController>> mockLogger;
        private Mock<IMapper> mockMapper;
        public DotNetMetricsControllerUnitTests()
        {
            mock = new Mock<IDotNetMetricsRepository>();
            mockLogger = new Mock<ILogger<DotNetMetricsAgentController>>();
            mockMapper = new Mock<IMapper>();
            _controller = new DotNetMetricsAgentController(mockLogger.Object, mock.Object, mockMapper.Object);
        }

        [Fact]
        public void GetAll_ShouldCall_GetAll_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<DotNetMetric>());
            var result = _controller.GetAll();
            mock.Verify(repository => repository.GetAll());


        }

        [Fact]
        public void GetErrorsCount_ShouldCall_GetErrorsCount_From_Repository()
        {
            var fromTime = new TimeSpan(00, 05, 00);
            var toTime = new TimeSpan(00, 10, 00);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Returns(new List<DotNetMetric>());
            var result = _controller.GetErrorsCount(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
