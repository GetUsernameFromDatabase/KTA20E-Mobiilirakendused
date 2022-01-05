using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Converters;
using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Models.Responses;
using WeatherApp.Models.Weather;
using WeatherApp.Services;
using WeatherApp.Views.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MainPageViewModel : BaseViewModel
    {
        public bool IsRefreshing { get; set; }

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

        private async void ShowAddLocationDialog()
        {
            var dialog = new AddLocationDialog();
            // New Location needs to be added
            dialog.CloseWhenBackgroundIsClicked = false;
            await PopupNavigation.Instance.PushAsync(dialog);
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
            // await MockThis();
            var ActiveCity = MainInfo?.City?.Name;
            if (ActiveCity == null)
            {
                var locations = await Locations.GetLocationsFromStorage();
                if (locations == null)
                {
                    ShowAddLocationDialog();
                    return;
                }
                else ActiveCity = locations.Single((x) => x.Selected).Locality;
            }

            var weatherData = WeatherService.Get5DayForecast(ActiveCity).Result;
            ApplyWeatherData(weatherData);
        }

        public void ApplyWeatherData(OWM_5Day3HourForecast weatherData)
        {
            var wdCity = weatherData.city;
            var wdCrnt = weatherData.list.First();
            var wdWeather = wdCrnt.weather.First();
            var wdMain = wdCrnt.main;
            var wdWind = wdCrnt.wind;

            var temp = new Temperature(wdMain.temp, TemperatureUnits.Celsius);
            var city = new Models.City() { Name = wdCity.name, Country = wdCity.country };
            var date = new Date(Time.UnixTimeStampToDateTime(wdCrnt.dt));
            MainInfo = new MainInfo(city, temp, date, wdWeather.icon) { Description = wdWeather.description };

            var wind = new Models.Weather.Wind(wdWind.speed, WindUnits.Metric);
            var pressure = new Pressure(wdMain.pressure, PressureUnits.hectopascal);
            DetailedInfo = new DetailedInfo(wind, pressure)
            {
                Cloudiness = wdCrnt.clouds.all,
                Humidity = wdMain.humidity,
            };

            List<WeatherForecast> weatherList = new List<WeatherForecast> { };
            var iterateDay = 24 / 3;
            for (int i = iterateDay; i < weatherData.list.Length; i += iterateDay)
            {
                var wdAfterADay = weatherData.list[i];
                var iTemp = new Temperature(wdAfterADay.main.temp, TemperatureUnits.Celsius);
                var iUnixTime = wdAfterADay.dt;
                var icon = wdAfterADay.weather.First().icon;

                var weatherForecast = new WeatherForecast(iTemp, iUnixTime, icon);
                weatherList.Add(weatherForecast);
            }
            WeatherForecasts = weatherList;
        }

        #region Mocking Weather Info

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "For Development")]
        private Task MockThis()
        {
            MainInfo = MockMainInfo();
            DetailedInfo = MockDetailedInfo();
            WeatherForecasts = MockWeatherData();
            return Task.Delay(1337);
        }

        private MainInfo MockMainInfo()
        {
            var temp = new Temperature(285.15, TemperatureUnits.Kelvin);
            temp.ConvertTo(TemperatureUnits.Celsius);

            var city = new Models.City() { Name = "London", Country = "GB" };
            var date = new Date(new DateTime(2019, 6, 15, 9, 3, 0));
            var desc = "Light intensity drizzle rain";
            return new MainInfo(city, temp, date, "09d") { Description = desc };
        }

        private DetailedInfo MockDetailedInfo()
        {
            var wind = new Models.Weather.Wind(2.6, WindUnits.Metric);
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