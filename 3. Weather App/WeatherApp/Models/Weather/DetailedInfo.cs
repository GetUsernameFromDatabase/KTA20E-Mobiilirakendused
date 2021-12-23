using WeatherApp.Models.Weather;

namespace WeatherApp.Models
{
    public class DetailedInfo
    {
        public int Humidity { get; set; }
        public int Cloudiness { get; set; }
        public Pressure Pressure { get; private set; }
        public Wind Wind { get; private set; }

        public DetailedInfo(Wind Wind, Pressure Pressure)
        {
            this.Pressure = Pressure;
            this.Wind = Wind;
        }
    }
}