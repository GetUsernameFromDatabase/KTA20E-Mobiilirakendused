using System.Collections.Generic;
using System.ComponentModel;
using System;
using Xamarin.Forms;
using WeatherApp.Models;
using WeatherApp.Models.Weather;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageView : ContentPage
    {
        public MainInfo MainInfo { get; set; }
        public DetailedInfo DetailedInfo { get; set; }
        public List<WeatherForecast> WeatherForecasts { get; set; }

        public MainPageView()
        {
            MockThis();
            BindingContext = this;
            InitializeComponent();
        }

        private void MockThis()
        {
            MainInfo = MockMainInfo();
            DetailedInfo = MockDetailedInfo();
            WeatherForecasts = MockWeatherData();
        }

        private MainInfo MockMainInfo()
        {
            var temp = new Temperature(285.15, TemperatureUnits.Kelvin);
            temp.ConvertTo(TemperatureUnits.Celsius);

            var city = new City() { Name = "London", Country = "GB" };
            var date = new Date(new DateTime(2019, 6, 15, 9, 3, 0));
            var desc = "Light intensity drizzle rain";
            return new MainInfo(city, temp, date) { Description = desc };
        }
        private DetailedInfo MockDetailedInfo()
        {
            var pressure = new Pressure(1000, PressureUnits.hectopascal);
            var wind = new Wind(15, WindUnits.Metric);

            return new DetailedInfo(wind, pressure)
            {
                Cloudiness = 100,
                Humidity = 50,
            };
        }

        private List<WeatherForecast> MockWeatherData()
        {
            var TempUnit = TemperatureUnits.Kelvin;
            List<WeatherForecast> weatherList = new List<WeatherForecast>
            {
                new WeatherForecast(new Temperature(295.15, TempUnit), 1560679200, "10d"),
                new WeatherForecast(new Temperature(294.15, TempUnit), 1560765600, "10d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), 1560852000, "10d"),
                new WeatherForecast(new Temperature(285.15, TempUnit), 1560938400, "10d"),
                new WeatherForecast(new Temperature(290.15, TempUnit), 1561024800, "10d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), 1561111200, "10d"),
            };

            foreach (var item in weatherList)
                item.Temperature.ConvertTo(TemperatureUnits.Celsius);
            return weatherList;
        }
    }
}
