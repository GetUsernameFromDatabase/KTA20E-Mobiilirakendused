using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BaseViewModel
    {
        #region Private & Protected

        private List<LocationModel> _locations;

        #endregion Private & Protected

        #region Properties

        public Command UseCurrentLocationCommand { get; set; }
        public bool HasError { get; set; }

        #endregion Properties

        #region Constructors

        public AddLocationDialogViewModel()
        {
            UseCurrentLocationCommand = new Command<string>(UseCurrentLocationCommandHandler);
            MainState = LayoutState.Loading;
        }

        #endregion Constructors

        #region Dialog

        public async void OnDialogOpened()
        {
            var listLocJson = await SecureStorage.GetAsync("locations");
            _locations = !string.IsNullOrEmpty(listLocJson)
                ? JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson)
                : new List<LocationModel>();
            MainState = LayoutState.None;
        }

        private async void UseCurrentLocationCommandHandler(string locationName)
        {
            MainState = LayoutState.Loading;
            HasError = false;

            var weatherData = WeatherService.Get5DayForecast(locationName).Result;
            Console.WriteLine(weatherData);
            if (weatherData == null)
            {
                HasError = true;
                MainState = LayoutState.None;
                return;
            }

            var city = weatherData.city;
            LocationModel location = new LocationModel()
            {
                Latitude = city.coord.lat,
                Longitude = city.coord.lon,
                CountryName = city.country,
                Locality = city.name,
                Selected = false,
            };

            _locations.Add(location);
            _locations.ForEach(l => l.Selected = false);
            _locations.First(l => l.Locality == location.Locality).Selected = true;
            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(_locations));
            MainState = LayoutState.None;
        }

        #endregion Dialog
    }
}