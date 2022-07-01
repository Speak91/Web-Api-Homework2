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
    public class HddMetricsAgentClient : IHddMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IAgentsRepository _agentsRepository;

        public HddMetricsAgentClient (HttpClient httpClient, 
            ILogger<HddMetricsAgentClient> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public HddMetricsResponse GetAllMetrics(HddMetricsRequest hddMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentsRepository.GetById(hddMetricsRequest.AgentId);
                //AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == hddMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{hddMetricsRequest.AgentId} not found.");

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/hdd/from/{hddMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{hddMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    HddMetricsResponse hddMetricsResponse = (HddMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(HddMetricsResponse));
                    hddMetricsResponse.AgentId = hddMetricsRequest.AgentId;
                    return hddMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public List<HddMetricsResponse> GetMetricsHddFromAllCluster(HddMetricsRequest hddMetricsRequest)
        {
            IList<AgentInfo> agents = _agentsRepository.Get();
            List<HddMetricsResponse> metrics = new List<HddMetricsResponse>(); 
            foreach (var item in agents)
            {
                hddMetricsRequest.AgentId = item.AgentId;
               metrics.Add(GetAllMetrics(hddMetricsRequest));
            }
            return metrics;
        }
    }
}
