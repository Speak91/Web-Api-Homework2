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
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        public void Create(RamMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("INSERT INTO  rammetrics(value,time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public void Update(RamMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("UPDATE rammetrics SET value = @value, time = @time WHERE id = @id",
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
            connection.Execute("DELETE FROM rammetrics WHERE id=@id",
            new
            {
                id = id
            });
        }

        public IList<RamMetric> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            IList<RamMetric> metrics = connection.Query<RamMetric>("SELECT Id, Time, Value FROM rammetrics").ToList();
            return metrics;
        }

        public RamMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            RamMetric metric = connection.QuerySingle<RamMetric>("SELECT Id, Time, Value FROM rammetrics",
                new { id = id });
            return metric;
        }

        public IList<RamMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            List<RamMetric> metric = connection.Query<RamMetric>("SELECT * FROM rammetrics WHERE (time >= @fromTime) and (time <= @toTime)",
                new
                {
                    fromTime = fromTime.TotalSeconds,
                    toTime = toTime.TotalSeconds
                }).ToList();
            return metric;
        }
    }
}
