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
    public class DotNetMetricsAgentClient : IDotNetMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IAgentsRepository _agentsRepository;

        public DotNetMetricsAgentClient (HttpClient httpClient, 
            ILogger<DotNetMetricsAgentClient> logger, 
            IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
            _logger = logger;
        }

        public DotNetMetricsResponse GetAllMetrics(DotNetMetricsRequest dotNetMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentsRepository.GetById(dotNetMetricsRequest.AgentId);
                //AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == cpuMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{dotNetMetricsRequest.AgentId} not found.");
       
                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/dotnet/errors-count/from/{dotNetMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{dotNetMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    DotNetMetricsResponse dotNetMetricsResponse = (DotNetMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(DotNetMetricsResponse));
                    dotNetMetricsResponse.AgentId = dotNetMetricsRequest.AgentId;
                    return dotNetMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public List<DotNetMetricsResponse> GetMetricsDotNetFromAllCluster(DotNetMetricsRequest dotNetMetricsRequest)
        {
            IList<AgentInfo> agents = _agentsRepository.Get();
            List<DotNetMetricsResponse> metrics = new List<DotNetMetricsResponse>(); 
            foreach (var item in agents)
            {
                dotNetMetricsRequest.AgentId = item.AgentId;
               metrics.Add(GetAllMetrics(dotNetMetricsRequest));
            }
            return metrics;
        }
    }
}
