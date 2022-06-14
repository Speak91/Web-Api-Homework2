using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _ramCounter =new PerformanceCounter("Memory", "% Committed Bytes In Use");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //значение занятости CPU
            float ramUsageInPercents = _ramCounter.NextValue();
            //время получения метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _ramMetricsRepository.Create(new RamMetric
            {
                Time = time.TotalSeconds,
                Value = (int)ramUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
