using MetricsManager.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Impl
{
    public interface INetworkMetricsAgentClient : IMetricsAgentClient<NetworkMetricsResponse, NetworkMetricsRequest>
    {
        List<NetworkMetricsResponse> GetMetricsNetworkFromAllCluster(NetworkMetricsRequest netwokrMetricsRequest);
    }
}
