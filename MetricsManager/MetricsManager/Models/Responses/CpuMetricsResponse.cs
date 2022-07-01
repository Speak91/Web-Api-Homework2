using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models.Request
{
    public class CpuMetricsResponse
    {
        public int AgentId { get; set; }
        public CpuMetric[] Metrics { get; set; }

    }
}
