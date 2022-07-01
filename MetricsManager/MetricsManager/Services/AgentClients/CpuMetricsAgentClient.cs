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
    public class CpuMetricsAgentClient : ICpuMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IAgentsRepository _agentsRepository;

        public CpuMetricsAgentClient (HttpClient httpClient, 
            ILogger<CpuMetricsAgentClient> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public CpuMetricsResponse GetAllMetrics(CpuMetricsRequest cpuMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentsRepository.GetById(cpuMetricsRequest.AgentId);
                //AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == cpuMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{cpuMetricsRequest.AgentId} not found.");

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/cpu/from/{cpuMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{cpuMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsResponse));
                    cpuMetricsResponse.AgentId = cpuMetricsRequest.AgentId;
                    return cpuMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public List<CpuMetricsResponse> GetMetricsCpuFromAllCluster(CpuMetricsRequest cpuMetricsRequest)
        {
            IList<AgentInfo> agents = _agentsRepository.Get();
            List<CpuMetricsResponse> metrics = new List<CpuMetricsResponse>(); 
            foreach (var item in agents)
            {
                cpuMetricsRequest.AgentId = item.AgentId;
               metrics.Add(GetAllMetrics(cpuMetricsRequest));
            }
            return metrics;
        }
    }
}
