using System;

namespace WeatherApp.Models
{
    public class Date
    {
        public DateTime DateTime { get; private set; }
        public bool amPM = true;
        public string Short => DateTime.ToString("dddd, dd");
        public string Detailed => amPM ?
            DateTime.ToString("MMMM dd, hh:mm tt") :
            DateTime.ToString("MMMM dd, HH:mm");

        public Date(DateTime DateTime)
        {
            this.DateTime = DateTime;
        }
    }
}
