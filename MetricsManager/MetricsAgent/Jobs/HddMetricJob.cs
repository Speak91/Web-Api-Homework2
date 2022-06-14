using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var hddUsageInPercents = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new HddMetric() { Time = time, Value = hddUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
