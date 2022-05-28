using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models.Request
{
    public class NetworkMetricsResponse
    {
        public int AgentId { get; set; }
        public NetworkMetric[] Metrics { get; set; }

    }
}
