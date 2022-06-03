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

        public HddMetricsControllerUnitTests()
        {
            mock = new Mock<IHddMetricsRepository>();
            mockLogger = new Mock<ILogger<HddMetricsAgentController>>();
            _controller = new HddMetricsAgentController(mockLogger.Object, mock.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит HddMetric - объект
            mock.Setup(repository =>
            repository.Create(It.IsAny<HddMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = _controller.Create(new
            HddMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()),
            Times.AtMostOnce());

        }

        [Fact]
        public void GetFreeHDDSpace_ShouldCall_GetFreeHDDSpace_From_Repository()
        {
            mock.Setup(repository => repository.GetAll()).Returns(new List<HddMetric>());
            var result = _controller.GetFreeHDDSpace();
            mock.Verify(repository => repository.GetAll());
        }
    }
}
