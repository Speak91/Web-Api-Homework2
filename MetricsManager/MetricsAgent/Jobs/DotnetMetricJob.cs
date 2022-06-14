using MetricsAgent.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class DotnetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;
        private readonly PerformanceCounter _counter;

        public DotnetMetricJob(IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown", "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var dotnetUsageInPercents = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new DotNetMetric() { Time = time, Value = dotnetUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
