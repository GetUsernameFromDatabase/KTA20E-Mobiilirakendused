using System;
using WeatherApp.Models.Weather;
using WeatherApp.Models;

namespace WeatherApp.Models
{
    public class MainInfo
    {
        public City City { get; private set; }
        public Temperature Temperature { get; private set; }
        public string Description { get; set; }

        public Date Date { get; private set; }
        public string DateDisplay => Date.Detailed;

        public MainInfo(City City, Temperature Temperature, Date Date)
        {
            this.Temperature = Temperature;
            this.City = City;
            this.Date = Date;
        }
    }
}
