using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class DotNetMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Value} - {Time}";
        }
    }
}
