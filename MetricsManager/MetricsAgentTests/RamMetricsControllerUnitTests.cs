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

        public RamMetricsControllerUnitTests()
        {
            mock = new Mock<IRamMetricsRepository>();
            mockLogger = new Mock<ILogger<RamMetricsAgentController>>();
            _controller = new RamMetricsAgentController(mockLogger.Object, mock.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {

            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит RamMetric - объект
            mock.Setup(repository =>
            repository.Create(It.IsAny<RamMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = _controller.Create(new
            RamMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(repository => repository.Create(It.IsAny<RamMetric>()),
            Times.AtMostOnce());

        }

        [Fact]
        public void GetAvailable_ShouldCall_GetAvailable_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<RamMetric>());
            var result = _controller.GetAvailable();
            mock.Verify(repository => repository.GetAll());
        }
    }
}
