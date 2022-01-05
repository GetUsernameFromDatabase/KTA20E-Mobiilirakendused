using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<WeatherForecast> WeatherForecasts { get; set; }

        #endregion WeatherInfo

        #region Commands

        public ICommand MenuCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        #endregion Commands

        #region Contructor

        public MainPageViewModel(MainPage Page)
        {
            MenuCommand = new Command(Page.OpenCloseMenu);
            RefreshCommand = new Command(RefreshCommandHandler);

            GetCurrentWeather().GetAwaiter();
        }

        // For Visual Studio !! Not to be used in production
        // https://docs.microsoft.com/en-us/answers/questions/612910/binding-property-34fromicao34-not-found-on-34viewm.html
        public MainPageViewModel()
        { throw new NotImplementedException("Not meant to be used"); }

        #endregion Contructor

        #region CommandHandlers

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

        #endregion CommandHandlers

        private async void ShowAddLocationDialog()
        {
            var dialog = new AddLocationDialog();
            // New Location needs to be added
            dialog.CloseWhenBackgroundIsClicked = false;
            await PopupNavigation.Instance.PushAsync(dialog);
        }

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

        public void ConvertToPreferredUnits()
        {
            var Units = Preferences.Get("units", "metric");
            string tempUnit, windUnit;
            switch (Units)
            {
                case "imperial":
                    tempUnit = TemperatureUnits.Fahrenheit;
                    windUnit = WindUnits.Imperial;
                    break;

                default:
                    tempUnit = TemperatureUnits.Celsius;
                    windUnit = WindUnits.Metric;
                    break;
            }

            MainInfo.Temperature.ConvertTo(tempUnit);
            DetailedInfo.Wind.ConvertTo(windUnit);

            foreach (var item in WeatherForecasts)
            {
                item.Temperature.ConvertTo(tempUnit);
                item.DetailedInfo.Wind.ConvertTo(windUnit);
            }
        }

        #region Making Weather Info

        public void ApplyWeatherData(OWM_5Day3HourForecast weatherData)
        {
            MainInfo = MakeMainInfo(weatherData);
            DetailedInfo = MakeDetailedInfo(weatherData.list.First());
            WeatherForecasts = MakeWeatherData(weatherData);
            ConvertToPreferredUnits();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members", Justification = "For Development")]
        private Task MockThis()
        {
            MainInfo = MakeMainInfo();
            DetailedInfo = MakeDetailedInfo();
            WeatherForecasts = MakeWeatherData();
            ConvertToPreferredUnits();
            return Task.Delay(1337);
        }

        #region Make MainInfo

        // Mock
        private MainInfo MakeMainInfo()
        {
            var temp = new Temperature(285.15, TemperatureUnits.Kelvin);
            temp.ConvertTo(TemperatureUnits.Celsius);

            var city = new Models.City() { Name = "London", Country = "GB" };
            var date = new Date(new DateTime(2019, 6, 15, 9, 3, 0));
            var desc = "Light intensity drizzle rain";
            return new MainInfo(city, temp, date, "09d") { Description = desc };
        }

        private MainInfo MakeMainInfo(OWM_5Day3HourForecast weatherData)
        {
            var wdCity = weatherData.city;
            var wdCrnt = weatherData.list.First();
            var wdWeather = wdCrnt.weather.First();

            var temp = new Temperature(wdCrnt.main.temp, TemperatureUnits.Celsius);
            var city = new Models.City() { Name = wdCity.name, Country = wdCity.country };
            var date = new Date(Time.UnixTimeStampToDateTime(wdCrnt.dt));
            return new MainInfo(city, temp, date, wdWeather.icon) { Description = wdWeather.description };
        }

        #endregion Make MainInfo

        #region Make DetailedInfo

        // Mock
        private DetailedInfo MakeDetailedInfo()
        {
            var wind = new Models.Weather.Wind(2.6, WindUnits.Metric);
            var pressure = new Pressure(1011, PressureUnits.hectopascal);

            return new DetailedInfo(wind, pressure)
            {
                Cloudiness = 24,
                Humidity = 50,
            };
        }

        private DetailedInfo MakeDetailedInfo(WeatherApp.Models.Responses.List wdCrnt)
        {
            var wdMain = wdCrnt.main;
            var wdWind = wdCrnt.wind;

            var wind = new Models.Weather.Wind(wdWind.speed, WindUnits.Metric);
            var pressure = new Pressure(wdMain.pressure, PressureUnits.hectopascal);
            return new DetailedInfo(wind, pressure)
            {
                Cloudiness = wdCrnt.clouds.all,
                Humidity = wdMain.humidity,
            };
        }

        #endregion Make DetailedInfo

        #region Make ObservableCollection<WeatherForecast>

        // Mock
        private ObservableCollection<WeatherForecast> MakeWeatherData()
        {
            var TempUnit = TemperatureUnits.Kelvin;
            var DI_Mock = MakeDetailedInfo(); // Detailed Info Mock
            ObservableCollection<WeatherForecast> weatherList = new ObservableCollection<WeatherForecast>
            {
                new WeatherForecast(new Temperature(295.15, TempUnit), DI_Mock, 1560679200, "10d"),
                new WeatherForecast(new Temperature(294.15, TempUnit), DI_Mock, 1560765600, "09d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), DI_Mock, 1560852000, "04d"),
                new WeatherForecast(new Temperature(285.15, TempUnit), DI_Mock, 1560938400, "04d"),
                new WeatherForecast(new Temperature(290.15, TempUnit), DI_Mock, 1561024800, "10d"),
                new WeatherForecast(new Temperature(293.15, TempUnit), DI_Mock, 1561111200, "09d"),
            };

            foreach (var item in weatherList)
                item.Temperature.ConvertTo(TemperatureUnits.Celsius);
            return weatherList;
        }

        private ObservableCollection<WeatherForecast> MakeWeatherData(OWM_5Day3HourForecast weatherData)
        {
            ObservableCollection<WeatherForecast> weatherList = new ObservableCollection<WeatherForecast> { };
            var iterateDay = 24 / 3;
            for (int i = iterateDay; i < weatherData.list.Length; i += iterateDay)
            {
                var item = weatherData.list[i];
                var temp = new Temperature(item.main.temp, TemperatureUnits.Celsius);
                var icon = item.weather.First().icon;
                var detailedInfo = MakeDetailedInfo(item);

                var weatherForecast = new WeatherForecast(temp, detailedInfo, item.dt, icon);
                weatherList.Add(weatherForecast);
            }
            return weatherList;
        }

        #endregion Make ObservableCollection<WeatherForecast>

        #endregion Making Weather Info
    }
}