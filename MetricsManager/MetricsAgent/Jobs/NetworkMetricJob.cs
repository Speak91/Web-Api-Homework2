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
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;
        private readonly PerformanceCounter _counter;
        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter("Process", "IO Read Bytes/sec", "System");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var networkUsageInPercents = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new NetworkMetric() { Time = time, Value = networkUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
