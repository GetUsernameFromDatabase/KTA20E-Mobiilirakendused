using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }

        public string FullName => string.Format("{0}, {1}", Name.ToUpper(), Country);
    }
}
