using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Models.Weather;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MainPageViewModel : BaseViewModel
    {
        public bool IsRefreshing { get; set; }
        public String ActiveCity { get; set; }

        #region WeatherInfo

        public MainInfo MainInfo { get; set; }
        public DetailedInfo DetailedInfo { get; set; }
        public List<WeatherForecast> WeatherForecasts { get; set; }

        #endregion WeatherInfo

        #region Commands

        public ICommand MenuCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        #endregion Commands

        public MainPageViewModel(MainPage Page)
        {
            MenuCommand = new Command(Page.OpenCloseMenu);
            RefreshCommand = new Command(RefreshCommandHandler);

            GetCurrentWeather().GetAwaiter();
        }

        #region Visual Studio Shenanigan

        // For Visual Studio !! Not to be used in production
        // https://docs.microsoft.com/en-us/answers/questions/612910/binding-property-34fromicao34-not-found-on-34viewm.html
        public MainPageViewModel()
        { throw new NotImplementedException("Not meant to be used"); }

        #endregion Visual Studio Shenanigan

        #region Command Handlers

        private async void RefreshCommandHandler()
        {
            IsRefreshing = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRefreshing = false;
                return;
            }

            await GetCurrentWeather();
            IsRefreshing = false;
        }

        #endregion Command Handlers

        private async Task GetCurrentWeather()
        {
            // Get Weather
            await Task.Delay(1000);
            MockThis();
        }

        #region Mocking Weather Info

        private void MockThis()
        {
            ActiveCity = "London";
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
            return new MainInfo(city, temp, date, "09d") { Description = desc };
        }

        private DetailedInfo MockDetailedInfo()
        {
            var wind = new Wind(2.6, WindUnits.Metric);
            var pressure = new Pressure(1011, PressureUnits.hectopascal);

            return new DetailedInfo(wind, pressure)
            {
                Cloudiness = 24,
                Humidity = 50,
            };
        }

        private List<WeatherForecast> MockWeatherData()
        {
            var TempUnit = TemperatureUnits.Kelvin;
            List<WeatherForecast> weatherList = new List<WeatherForecast>
            {
                new WeatherForecast(new Temperature(295.15, TempUnit), 1560679200, "10d"),
                new WeatherForecast(new Temperature(294.15, TempUnit), 1560765600, "09d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), 1560852000, "04d"),
                new WeatherForecast(new Temperature(285.15, TempUnit), 1560938400, "04d"),
                new WeatherForecast(new Temperature(290.15, TempUnit), 1561024800, "10d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), 1561111200, "09d"),
            };

            foreach (var item in weatherList)
                item.Temperature.ConvertTo(TemperatureUnits.Celsius);
            return weatherList;
        }

        #endregion Mocking Weather Info
    }
}