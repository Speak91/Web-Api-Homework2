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
        public void Create_ShouldCall_Create_From_Repository()
        {
            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит DotNetMetric - объект
            mock.Setup(repository =>
            repository.Create(It.IsAny<DotNetMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = _controller.Create(new
            DotNetMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()),
            Times.AtMostOnce());

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
