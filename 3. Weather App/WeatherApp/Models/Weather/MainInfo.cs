using WeatherApp.Converters;
using WeatherApp.Models.Weather;

namespace WeatherApp.Models
{
    public class MainInfo
    {
        public City City { get; private set; }
        public Temperature Temperature { get; private set; }
        public string Description { get; set; }
        public string IconPath { get; private set; }

        public Date Date { get; private set; }
        public string DateDisplay => Date.Detailed;

        public MainInfo(City City, Temperature Temperature, Date Date, string icon = null)
        {
            this.Temperature = Temperature;
            this.City = City;
            this.Date = Date;
            if (icon != null) this.IconPath = Icons.GetWeatherIcon(icon);
        }
    }
}