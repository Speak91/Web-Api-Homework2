using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Services.Interfaces
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {
        IList<HddMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
    }
}
