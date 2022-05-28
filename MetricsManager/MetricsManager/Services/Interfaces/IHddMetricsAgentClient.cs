using MetricsManager.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Impl
{
    public interface IHddMetricsAgentClient : IMetricsAgentClient<HddMetricsResponse, HddMetricsRequest>
    {
        List<HddMetricsResponse> GetMetricsHddFromAllCluster(HddMetricsRequest hddMetricsRequest);
    }
}
