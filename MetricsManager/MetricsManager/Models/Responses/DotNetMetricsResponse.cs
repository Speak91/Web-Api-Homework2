using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models.Request
{
    public class DotNetMetricsResponse
    {
        public int AgentId { get; set; }
        public DotNetMetric[] Metrics { get; set; }

    }
}
