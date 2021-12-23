namespace WeatherApp.Models.Weather
{
    public struct PressureUnits : IMeasurementUnits
    {
        public static readonly string hectopascal = "hpa";

        public string[] Units => new string[1] { hectopascal };
    }

    public class Pressure : Measurement
    {
        public Pressure(double value, string unit) : base(value, unit)
        {
            // this.MeasurementUnit = new PressureUnit(); // Currently not needed
        }
    }
}