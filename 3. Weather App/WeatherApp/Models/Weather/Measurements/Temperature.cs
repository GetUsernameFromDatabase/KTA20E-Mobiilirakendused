using System;

namespace WeatherApp.Models.Weather
{
    public readonly struct TemperatureUnits : IMeasurementUnits
    {
        public static readonly string Kelvin = "°K";
        public static readonly string Celsius = "°C";
        public static readonly string Fahrenheit = "°F";

        public string[] Units => new string[3] { Kelvin, Celsius, Fahrenheit };
        public Func<double, bool, double>[] UnitConversions => new Func<double, bool, double>[]
        {
            Kelvin_Celsius,
            Celsius_Fahrenheit
        };

        public double Kelvin_Celsius(double value, bool leftToRight)
        {
            var delta = 273.15;
            return leftToRight ?
                value - delta :
                value + delta;
        }

        public double Celsius_Fahrenheit(double value, bool leftToRight)
        {
            var delta = 32;
            return leftToRight ?
                value * 1.80 + delta :
                (value - delta) / 1.8;
        }
    };

    public class Temperature : Measurement
    {
        public Temperature(double value, string unit) : base(value, unit)
        {
            this.MeasurementUnit = new TemperatureUnits();
        }
    }
}

