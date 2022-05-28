using MetricsManager.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Impl
{
    public interface IDotNetMetricsAgentClient : IMetricsAgentClient<DotNetMetricsResponse, DotNetMetricsRequest>
    {
        List<DotNetMetricsResponse> GetMetricsDotNetFromAllCluster(DotNetMetricsRequest dotNetMetricsRequest);
    }
}
