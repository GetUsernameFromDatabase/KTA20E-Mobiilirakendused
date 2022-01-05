using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Models.Responses;
using WeatherApp.Services;
using WeatherApp.Views;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BaseViewModel
    {
        #region Private & Protected

        private AddLocationDialog Page;

        private List<LocationModel> _locations;

        #endregion Private & Protected

        #region Properties

        public Command UseCurrentLocationCommand { get; set; }
        public bool HasError { get; set; }

        #endregion Properties

        #region Constructors

        public AddLocationDialogViewModel(AddLocationDialog Page)
        {
            this.Page = Page;
            UseCurrentLocationCommand = new Command<string>(UseCurrentLocationCommandHandler);
            MainState = LayoutState.Loading;
        }

        public AddLocationDialogViewModel()
        { throw new NotImplementedException("Not meant to be used"); }

        #endregion Constructors

        #region Dialog

        public async void OnDialogOpened()
        {
            _locations = await Locations.GetLocationsFromStorage() ?? new List<LocationModel>();
            MainState = LayoutState.None;
        }

        private async void UseCurrentLocationCommandHandler(string locationName)
        {
            MainState = LayoutState.Loading;
            HasError = false;

            var weatherData = WeatherService.Get5DayForecast(locationName).Result;
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

            await AddLocation(location);
            UpdatePages(weatherData).GetAwaiter();

            MainState = LayoutState.None;
            // Closes self
            await PopupNavigation.Instance.PopAsync();
        }

        private Task UpdatePages(OWM_5Day3HourForecast weatherData)
        {
            var navPage = App.Current.MainPage as NavigationPage;

            var mainPage = navPage.RootPage as MainPage;
            mainPage.viewModel.ApplyWeatherData(weatherData);

            var crntPage = navPage.CurrentPage as NavigationPage;
            if (crntPage?.RootPage is LocationsPage locPage)
                return locPage.viewModel.GetPlacemarkAndLocation();

            return null;
        }

        private Task AddLocation(LocationModel location)
        {
            _locations.Add(location);
            _locations.ForEach(l => l.Selected = false);
            _locations.First(l => l.Locality == location.Locality).Selected = true;
            return Locations.SaveLocationsToStorage(_locations);
        }

        #endregion Dialog
    }
}