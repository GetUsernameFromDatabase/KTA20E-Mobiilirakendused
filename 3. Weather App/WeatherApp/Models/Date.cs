using System;

namespace WeatherApp.Models
{
    public class Date
    {
        public bool amPM = false;

        public DateTime DateTime { get; private set; }
        public DateTime LocalTime => DateTime.ToLocalTime();

        public string Short => LocalTime.ToString("dddd, dd");

        public string Detailed => amPM ?
            LocalTime.ToString("MMMM dd, hh:mm tt") :
            LocalTime.ToString("MMMM dd, HH:mm");

        public Date(DateTime DateTime)
        {
            this.DateTime = DateTime;
        }
    }
}