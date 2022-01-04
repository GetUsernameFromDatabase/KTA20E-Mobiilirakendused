using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models.Responses;

namespace WeatherApp.Services
{
    public struct WeatherService
    {
        private static readonly string BaseURL = @"https://api.openweathermap.org/data/2.5/";
        private static readonly string ApiKey = "a519d2565f58343b5f157d056e658aca";

        public static async Task<OWM_5Day3HourForecast> Get5DayForecast(string city)
        {
            HttpClient client = new HttpClient();
            var reqUrl = $"{BaseURL}forecast?q={city}&units=metric&appid={ApiKey}";
            Debug.WriteLine(reqUrl);

            try
            {
                string response = client.GetStringAsync(reqUrl).Result;
                var data = JsonConvert.DeserializeObject<OWM_5Day3HourForecast>(response);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error with OpenWeatherMap Service:");
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}