using Newtonsoft.Json;
using System.Net.Http;
using System;
using WeatherApp.Models.Responses;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    class WeatherService
    {
        private readonly string BaseURL = @"https://api.openweathermap.org/data/2.5/";
        private readonly string ApiKey = "148ff5a63e92f025856f0912aacd4b80";

        public async Task<OWM_5Day3HourForecast> Get5DayForecast(string city)
        {
            HttpClient client = new HttpClient();
            try
            {
                string response = await client.GetStringAsync
                ($"{BaseURL}weather?q={city}&units=metric&appid={ApiKey}");
                var data = JsonConvert.DeserializeObject<OWM_5Day3HourForecast>(response);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
