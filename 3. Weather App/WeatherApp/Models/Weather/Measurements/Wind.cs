using System;

namespace WeatherApp.Models.Weather
{
    public struct WindUnits : IMeasurementUnits
    {
        public static readonly string Metric = "m/s";
        public static readonly string Imperial = "mph";

        public string[] Units => new string[2] { Metric, Imperial };
        public Func<double, bool, double>[] UnitConversions => new Func<double, bool, double>[]
        {
            Metric_Imperial,
        };

        public double Metric_Imperial(double value, bool leftToRight)
        {
            var mlt = 2.236936;
            return leftToRight ?
                value / mlt :
                value * mlt;
        }
    }

    public class Wind : Measurement
    {
        public Wind(double value, string unit) : base(value, unit)
        {
            this.MeasurementUnit = new WindUnits();
        }
    }
}
