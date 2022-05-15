using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork_1.Models
{
    public class ValuesHolder
    {
        private List<Weather> _weathers;

        public ValuesHolder()
        {
            _weathers = new List<Weather>();
        }

        public void Add(Weather weather)
        {
            if (weather != null || !_weathers.Contains(weather))
                _weathers.Add(weather);

        }

        public void Update(DateTime time, int temperature)
        {
            for (int i = 0; i < _weathers.Count; i++)
            {
                if (_weathers[i].Date == time)
                {
                    _weathers[i].TemperatureC = temperature;
                }
            }
        }

        public void Delete(DateTime fromTime, DateTime toTime)
        {
            if (fromTime > toTime)
            {
                throw new Exception("Неверный диапазон дат");
            }
            for (int i = 0; i < _weathers.Count; i++)
            {
                if (_weathers[i].Date >= fromTime && _weathers[i].Date <=toTime)
                {
                    _weathers.RemoveAt(i);
                }
            }
        }

        public IEnumerable<Weather> GetTemperatures(DateTime beginTime, DateTime endTime)
        {
            if (_weathers != null || endTime > beginTime)
            {
                foreach (var weather in _weathers.ToList())
                {
                    if (weather.Date >= beginTime && weather.Date <= endTime)
                    {
                        yield return weather;
                    }
                }
            }

            else
            {
                yield return null;
            }
        }

        /// <summary>
        /// Метод с помощью которого добавлял случ-ые значения для тестирования всего приложения
        /// </summary>
        /// <param name="a"></param>
        public void Random(int a)
        {
            Random random = new Random();
            for (int i = 0; i != a; i++)
            {
                DateTime start = new DateTime(1995, 1, 1);
                int range = (DateTime.Today - start).Days;
                _weathers.Add(new Weather { TemperatureC = random.Next(-100, 100), Date = start.AddDays(random.Next(range)) });
            }

        }
    }
}
