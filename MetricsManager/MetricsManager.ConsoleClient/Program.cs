using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MetricsManager.ConsoleClient
{
    public class GetMetrics
    {
        private MetricsManagerConsoleClient _metricsManagerClient;
        private CpuMetricsRequest _cpuMetricsRequest;
        private HddMetricsRequest _hddMetricsRequest;
        private RamMetricsRequest _ramMetricsRequest;
        private DotNetMetricsRequest _dotNetMetricsRequest;
        private NetworkMetricsRequest _networkMetricsRequest;
        public GetMetrics()
        {
            _metricsManagerClient = new MetricsManagerConsoleClient("http://localhost:10959", new HttpClient());

        }

        public CpuMetricsResponse GetCpuMetricsFromAgent()
        {
            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);
                _cpuMetricsRequest = new CpuMetricsRequest()
                {
                    AgentId = 4,
                    FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                };
                CpuMetricsResponse cpuMetrics = _metricsManagerClient.GetCpuMetricsFromAgentAsync(_cpuMetricsRequest).Result;
                return (cpuMetrics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
           
        }

        public HddMetricsResponse GetHddMetricsFromAgent()
        {
            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);
               _hddMetricsRequest = new HddMetricsRequest()
                {
                    AgentId = 4,
                    FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                };
                HddMetricsResponse metrics = _metricsManagerClient.GetHddMetricsFromAgentAsync(_hddMetricsRequest).Result;
                return (metrics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public RamMetricsResponse GetRamMetricsFromAgent()
        {
            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);
                _ramMetricsRequest = new RamMetricsRequest()
                {
                    AgentId = 4,
                    FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                };
                RamMetricsResponse metrics = _metricsManagerClient.GetRamMetricsFromAgentAsync(_ramMetricsRequest).Result;
                return (metrics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public DotNetMetricsResponse GetDotNetMetricsFromAgent()
        {
            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);
                _dotNetMetricsRequest = new DotNetMetricsRequest()
                {
                    AgentId = 4,
                    FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                };
                DotNetMetricsResponse metrics = _metricsManagerClient.ErrorsCountFromAgentAsync(_dotNetMetricsRequest).Result;
                return (metrics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public NetworkMetricsResponse GetNetworkMetricsFromAgent()
        {
            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);
                _networkMetricsRequest = new NetworkMetricsRequest()
                {
                    AgentId = 4,
                    FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                };
                NetworkMetricsResponse metrics = _metricsManagerClient.GetNetworkMetricsFromAgentAsync(_networkMetricsRequest).Result;
                return (metrics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GetMetrics metrics = new GetMetrics();
            while (true)
            {
                Console.WriteLine("Задачи");
                Console.WriteLine("=============================================");
                Console.WriteLine("1 - Получить метрики за последнюю минуту CPU");
                Console.WriteLine("2 - Получить метрики за последнюю минуту DOTNET");
                Console.WriteLine("3 - Получить метрики за последнюю минуту HDD");
                Console.WriteLine("4 - Получить метрики за последнюю минуту NETWORK");
                Console.WriteLine("5 - Получить метрики за последнюю минуту RAM");
                Console.WriteLine("0 - Завершение работы приложения");
                Console.WriteLine("=============================================");
                Console.Write("Введите номер задачи");
                if (int.TryParse(Console.ReadLine(), out int taskNumber))
                {
                    switch (taskNumber)
                    {
                        case 0:
                            Console.WriteLine("Завершение работы");
                            return;
                        case 1:
                            CpuMetricsResponse cpuMetrics = metrics.GetCpuMetricsFromAgent();
                            foreach (var cpuMetric in cpuMetrics.Metrics)
                            {
                                Console.WriteLine($"" +
                                       $"{TimeSpan.Parse(cpuMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} > {cpuMetric.Value}");
                            }
                            break;

                        case 2:
                            DotNetMetricsResponse dotNetMetrics = metrics.GetDotNetMetricsFromAgent();
                            foreach (var dotNetMetric in dotNetMetrics.Metrics)
                            {
                                Console.WriteLine($"" +
                                       $"{TimeSpan.Parse(dotNetMetric .Time).ToString("dd\\.hh\\:mm\\:ss")} > {dotNetMetric.Value}");
                            }
                            break;

                        case 3:
                            HddMetricsResponse hddMetrics = metrics.GetHddMetricsFromAgent();
                            foreach (var hddMetric in hddMetrics.Metrics)
                            {
                                Console.WriteLine($"" +
                                       $"{TimeSpan.Parse(hddMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} > {hddMetric.Value}");
                            }
                            break;

                        case 4:
                            NetworkMetricsResponse networkMetrics = metrics.GetNetworkMetricsFromAgent();
                            foreach (var networkMetric in networkMetrics.Metrics)
                            {
                                Console.WriteLine($"" +
                                       $"{TimeSpan.Parse(networkMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} > {networkMetric.Value}");
                            }
                            break;

                        case 5:
                            RamMetricsResponse ramMetrics = metrics.GetRamMetricsFromAgent();
                            foreach (var ramMetric in ramMetrics.Metrics)
                            {
                                Console.WriteLine($"" +
                                       $"{TimeSpan.Parse(ramMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} > {ramMetric.Value}");
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Не коректный номер задачи");
                }
            }
        }
    }
}
