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
    public class NetworkMetricsAgentClient : INetworkMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IAgentsRepository _agentsRepository;

        public NetworkMetricsAgentClient (HttpClient httpClient, 
            ILogger<NetworkMetricsAgentClient> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public NetworkMetricsResponse GetAllMetrics(NetworkMetricsRequest networkMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentsRepository.GetById(networkMetricsRequest.AgentId);
                //AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == networkMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{networkMetricsRequest.AgentId} not found.");
             
                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/network/from/{networkMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{networkMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    NetworkMetricsResponse networkMetricsResponse = (NetworkMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(NetworkMetricsResponse));
                    networkMetricsResponse.AgentId = networkMetricsRequest.AgentId;
                    return networkMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public List<NetworkMetricsResponse> GetMetricsNetworkFromAllCluster(NetworkMetricsRequest networkMetricsRequest)
        {
            IList<AgentInfo> agents = _agentsRepository.Get();
            List<NetworkMetricsResponse> metrics = new List<NetworkMetricsResponse>(); 
            foreach (var item in agents)
            {
                networkMetricsRequest.AgentId = item.AgentId;
               metrics.Add(GetAllMetrics(networkMetricsRequest));
            }
            return metrics;
        }
    }
}
