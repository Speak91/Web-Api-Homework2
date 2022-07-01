 using MetricsManager.Models;
using MetricsManager.Models.Request;
using MetricsManager.Services.Impl;
using MetricsManager.Services.Interfaces;
using MetricsManager.Services.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MetricsManager.Services
{
    public class RamMetricsAgentClient : IRamMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IAgentsRepository _agentsRepository;

        public RamMetricsAgentClient (HttpClient httpClient, 
            ILogger<RamMetricsAgentClient> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public RamMetricsResponse GetAllMetrics(RamMetricsRequest ramMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentsRepository.GetById(ramMetricsRequest.AgentId);
                //AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == ramMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{ramMetricsRequest.AgentId} not found.");

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/ram/from/{ramMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{ramMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    RamMetricsResponse ramMetricsResponse = (RamMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(RamMetricsResponse));
                    ramMetricsResponse.AgentId = ramMetricsRequest.AgentId;
                    return ramMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public List<RamMetricsResponse> GetMetricsRamFromAllCluster(RamMetricsRequest ramMetricsRequest)
        {
            IList<AgentInfo> agents = _agentsRepository.Get();
            List<RamMetricsResponse> metrics = new List<RamMetricsResponse>(); 
            foreach (var item in agents)
            {
                ramMetricsRequest.AgentId = item.AgentId;
               metrics.Add(GetAllMetrics(ramMetricsRequest));
            }
            return metrics;
        }
    }
}
