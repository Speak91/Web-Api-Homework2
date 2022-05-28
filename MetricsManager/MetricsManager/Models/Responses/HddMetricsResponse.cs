using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models.Request
{
    public class HddMetricsResponse
    {
        public int AgentId { get; set; }
        public HddMetric[] Metrics { get; set; }

    }
}
