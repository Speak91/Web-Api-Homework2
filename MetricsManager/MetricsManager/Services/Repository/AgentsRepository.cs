using AutoMapper;
using Dapper;
using MetricsManager.Models;
using MetricsManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Services.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        private readonly ILogger<AgentsRepository> _logger;
        private IMapper _mapper;
        public AgentsRepository(ILogger<AgentsRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public void Create(string agentUrl)
        {
            using (var connection = new SQLiteConnection(SQLConnectionString.ConnectionString))
            {   
                var count = connection.ExecuteScalar<int>($"SELECT Count(*) FROM agents WHERE uri=@uri;", new { uri = agentUrl});
                if (count > 0)
                {
                    throw new ArgumentException("Агент уже существует");
                }
                var result = connection.Execute(
                $"INSERT INTO agents (uri,isenabled) VALUES (@uri,@IsEnabled);",
                new 
                { 
                    uri = agentUrl , 
                    IsEnabled = true
                }
                );
                if (result <= 0) throw new InvalidOperationException("Не удалось добавить агента.");
            }
        }

        public IList<AgentInfo> Get()
        {
            List<AgentInfo> agents = new List<AgentInfo>();
            using (var connection = new SQLiteConnection(SQLConnectionString.ConnectionString))
            {

                foreach (var item in connection.Query($"SELECT * FROM agents"))
                {
                    agents.Add(new AgentInfo()
                    {
                        AgentId = (int)item.Id,
                        AgentAddress = new Uri(item.Uri),
                        Enable = Convert.ToBoolean(item.IsEnabled)
                    });
                }

                return agents;
            }
           
        }

        public AgentInfo GetById(int id)
        {
            using (var connection = new SQLiteConnection(SQLConnectionString.ConnectionString))
            {
                AgentInfo agentInfo = new AgentInfo();
                return agentInfo = _mapper.Map<AgentInfo> (connection.QuerySingle<AgentInfoResponse>($"SELECT Id, Uri FROM agents WHERE id=@id",
                    new { id }));
            }
        }

        public void Update(AgentInfo agent)
        {
            using (var connection = new SQLiteConnection(SQLConnectionString.ConnectionString))
            {
                var count = connection.ExecuteScalar<int>($"SELECT Count(*) FROM agents WHERE uri=@uri;", new { uri = agent.AgentAddress });
                if (count <= 0)
                {
                    throw new ArgumentException("Агент не существует");
                }
                var result = connection.Execute($"UPDATE agents SET uri=@uri, isenabled=@isenabled where id=@id;",
                new 
                { 
                    uri = agent.AgentId, 
                    isenabled = agent.AgentAddress.ToString(),
                    id = agent.AgentId
                }
                );
                if (result <= 0) throw new InvalidOperationException("Не удалось обновить агента.");
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(SQLConnectionString.ConnectionString))
            {
                var count = connection.ExecuteScalar<int>($"SELECT Count(*) FROM agents WHERE id=@id;", new { id = id });
                if (count < 1)
                {
                    throw new ArgumentException("Агент не существует");
                }
                var result = connection.Execute($"DELETE FROM agents WHERE id=@id;", new {id =id});
                if (result <= 0) throw new InvalidOperationException("Не удалось удалить агента.");
            }
        }

    }
}
