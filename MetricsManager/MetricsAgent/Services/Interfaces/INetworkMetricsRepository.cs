using MetricsAgent.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Interfaces
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
        IList<NetworkMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
    }
}
