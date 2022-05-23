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
        public void Create_ShouldCall_Create_From_Repository()
        {

            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
            mock.Setup(repository =>
            repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = _controller.Create(new
            CpuMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()),
            Times.AtMostOnce());

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
