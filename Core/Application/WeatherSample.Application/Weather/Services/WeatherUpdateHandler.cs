using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherSample.Application.Interfaces;

namespace WeatherSample.Application.Weather.Services
{
    public class WeatherUpdateService : IWeatherUpdate
    {
        private readonly IWeatherDbContext dbContext;
        private readonly IConfiguration configuration;

        public WeatherUpdateService(IWeatherDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }
        public void UpdateWeather(string cityName)
        {
            try
            {
                var apiKey = configuration["WeatherApiKey"];
                var url = @$"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}&aqi=no";
                var client = new HttpClient();

                //todo make some log here if it fails
                var result = client.GetAsync(url).Result;
                var content = result.Content.ReadAsStringAsync().Result;

                var responseObject = JObject.Parse(content);
                var temp = responseObject["current"].Value<decimal>("temp_c");

                try
                {
                    sendRabbitMq(new
                    {
                        cityName = cityName,
                        temperature = temp
                    });
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot send rabbitmq message", e);
                }

                if (temp > 14)
                {
                    dbContext.CityWeatherDatas.Add(new Domain.CityWeatherData()
                    {
                        CityName = cityName,
                        RecordTime = DateTime.Now,
                        Temperature = temp
                    });

                    dbContext.SaveChanges();
                }
            }
            catch
            {
                //todo make some log here if it fails
                throw;
            }
        }

        void sendRabbitMq(object data)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var message = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                // Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
