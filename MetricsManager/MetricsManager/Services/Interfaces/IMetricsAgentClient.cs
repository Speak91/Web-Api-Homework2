using MetricsManager.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Impl
{
    public interface IMetricsAgentClient<T, R> where T : class
    {
        T GetAllMetrics(R request);
    }
}
