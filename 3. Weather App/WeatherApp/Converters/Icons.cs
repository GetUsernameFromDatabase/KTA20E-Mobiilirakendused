using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Converters
{
    public readonly struct Icons
    {
        public static string GetWeatherIcon(string icon)
        {
            return string.Format("OWM_{0}.png", icon);
        }
    }
}