using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Interfaces
{
    public interface IAgentsRepository
    {
        void Create(string agentUrl);
        IList<AgentInfo> Get();
        AgentInfo GetById(int id);
        void Update(AgentInfo agent);
        void Delete(int id);
    }
}
