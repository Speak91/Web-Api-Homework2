using MetricsManager.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Impl
{
    public interface ICpuMetricsAgentClient : IMetricsAgentClient<CpuMetricsResponse, CpuMetricsRequest>
    {
        List<CpuMetricsResponse> GetMetricsCpuFromAllCluster(CpuMetricsRequest cpuMetricsRequest);
    }
}
