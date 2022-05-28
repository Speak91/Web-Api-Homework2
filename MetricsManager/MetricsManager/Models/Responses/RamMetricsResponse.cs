using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models.Request
{
    public class RamMetricsResponse
    {
        public int AgentId { get; set; }
        public RamMetric[] Metrics { get; set; }

    }
}
