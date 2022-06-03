using Dapper;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Services.Repository
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        public void Create(HddMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("INSERT INTO  hddmetrics(value,time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }
        public void Update(HddMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("DELETE FROM hddmetrics WHERE id=@id",
            new
            {
                id = id
            });
        }

        public IList<HddMetric> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            IList<HddMetric> metrics = connection.Query<HddMetric>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            return metrics;
        }

        public HddMetric GetById(int id)
        {

            using var connection = new SQLiteConnection(ConnectionString);
            HddMetric metric = connection.QuerySingle<HddMetric>("SELECT Id, Time, Value FROM hddmetrics",
                new { id = id });
            return metric;
        }

        public IList<HddMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            List<HddMetric> metric = connection.Query<HddMetric>("SELECT * FROM hddmetrics WHERE (time >= @fromTime) and (time <= @toTime)",
                new
                {
                    fromTime = fromTime.TotalSeconds,
                    toTime = toTime.TotalSeconds
                }).ToList();
            return metric;
        }
    }
}
