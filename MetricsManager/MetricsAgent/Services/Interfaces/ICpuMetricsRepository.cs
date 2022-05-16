using System;
using System.Collections.Generic;

namespace MetricsAgent.Services
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
        IList<CpuMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
    }

}
