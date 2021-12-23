using WeatherApp.Converters;
using WeatherApp.Models.Weather;

namespace WeatherApp.Models
{
    public class WeatherForecast
    {
        public Temperature Temperature { get; private set; }
        public string IconPath { get; private set; }

        public Date MyDate { get; private set; }
        public string DateDisplay => MyDate.Short;

        public WeatherForecast(Temperature Temperature, double unixTime, string icon = "Error")
        {
            this.Temperature = Temperature;
            this.MyDate = new Date(Time.UnixTimeStampToDateTime(unixTime));
            this.IconPath = icon == "Error" ? "Loading.gif" : "OWM_" + icon + "-2x.png";
        }
    }
}