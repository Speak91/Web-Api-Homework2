using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MetricsManager.Wpf.Client
{
    /// <summary>
    /// Логика взаимодействия для DotNetChartControl.xaml
    /// </summary>
    public partial class DotNetChartControl : UserControl, INotifyPropertyChanged
    {
        private MetricsManagerClient _metricsManagerClient;
        private SeriesCollection _columnSeriesValues;
        private string _persentText;
        private string _persentTextDesciption;

        public DotNetChartControl()
        {
            InitializeComponent();
            _metricsManagerClient = new MetricsManagerClient("http://localhost:10959", new HttpClient());
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SeriesCollection ColumnSeriesValues
        {
            get
            {
                return _columnSeriesValues;
            }
            set
            {
                _columnSeriesValues = value;
                OnPropertyChanged("ColumnSeriesValues");
            }
        }

        public string PersentText
        {
            get
            {
                return _persentText;
            }
            set
            {
                _persentText = value;
                OnPropertyChanged("PersentText");
            }
        }

        public string PersentTextDesciption
        {
            get
            {
                return _persentTextDesciption;
            }
            set
            {
                _persentTextDesciption = value;
                OnPropertyChanged("PersentTextDesciption");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            Task.Run(() => {
                while (true)
                {
                    TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // new
                    TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                    DotNetMetricsRequest dotNetMetricsRequest = new DotNetMetricsRequest()
                    {
                        AgentId = 4,
                        FromTime = fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                        ToTime = toTime.ToString("dd\\.hh\\:mm\\:ss")
                    };

                    try
                    {
                        DotNetMetricsResponse dotNetMetricsResponse =
                               _metricsManagerClient.ErrorsCountFromAgentAsync(dotNetMetricsRequest).Result;
                        DotNetMetric[] metrics = dotNetMetricsResponse.Metrics.ToArray();

                        Dispatcher.Invoke(() =>
                        {
                            if (dotNetMetricsResponse.Metrics.Count > 0)
                            {
                                TimeSpan del = TimeSpan.Parse(metrics[metrics.Count() - 1].Time) - TimeSpan.Parse(metrics[0].Time);
                                PersentTextDesciption = $"За последние {del.TotalSeconds} сек. средняя загрузка";
                                double sum = (double)metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x);
                                PersentText = $"{sum / metrics.Count():F2}";
                            }

                            ColumnSeriesValues = new SeriesCollection
                            {
                                new ColumnSeries
                                {
                                    Values = new ChartValues<int>(metrics.Where(x => x != null).Select(x => x.Value).ToArray())
                                }
                            };

                            TimePowerChart.Update(true);
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Произошла ошибка при попытке получить CPU метрики.\n{ex.Message}");
                    }

                    Thread.Sleep(5000);

                }
            });
        }
    }
}
