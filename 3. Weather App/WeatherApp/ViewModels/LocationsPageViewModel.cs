using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Views;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WeatherApp.ViewModels
{
    public class LocationsPageViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<LocationModel> Locations { get; set; }

        #endregion Properties

        #region Commands

        public Command BackCommand { get; set; }
        public Command AddLocationCommand { get; set; }
        public Command SelectLocationCommand { get; set; }
        public Command DeleteLocationCommand { get; set; }

        #endregion Commands

        #region Constructors

        public LocationsPageViewModel(LocationsPage LocationsPage)
        {
            BackCommand = new Command(LocationsPage.BackCommandHandler);
            AddLocationCommand = new Command(AddLocationCommandHandler);
            SelectLocationCommand = new Command<string>(SelectLocationCommandHandler);
            DeleteLocationCommand = new Command<string>(DeleteLocationCommandHandler);

            Locations = new ObservableCollection<LocationModel>();
        }

        public LocationsPageViewModel()
        { throw new NotImplementedException("Not meant to be used"); }

        #endregion Constructors

        #region Command Handlers

        private async void AddLocationCommandHandler()
        {
            await PopupNavigation.Instance.PushAsync(new AddLocationDialog());
        }

        private async void SelectLocationCommandHandler(string selectedLocality)
        {
            MainState = LayoutState.Loading;
            foreach (var l in Locations)
            {
                if (l.Locality == selectedLocality)
                {
                    l.Selected = true;
                }
                else l.Selected = false;
            }
            Locations.RemoveAt(Locations.Count - 1);

            await Helpers.Locations.SaveLocationsToStorage(Locations);
            GetPlacemarkAndLocation().GetAwaiter();
            UpdateMainPage(selectedLocality);

            MainState = LayoutState.None;
        }

        private async void DeleteLocationCommandHandler(string selectedLocality)
        {
            MainState = LayoutState.Loading;

            if (Locations.Count > 2)
            {
                var item = Locations.First(l => l.Locality == selectedLocality);
                if (item.Selected)
                {
                    var index = Locations.IndexOf(item);
                    var nextIndex = index < Locations.Count - 2 ? index + 1 : 0;
                    var nextItem = Locations[nextIndex];

                    nextItem.Selected = true;
                    UpdateMainPage(nextItem.Locality);
                }
                Locations.Remove(item);
                Locations.RemoveAt(Locations.Count - 1);

                await Helpers.Locations.SaveLocationsToStorage(Locations);
                await GetPlacemarkAndLocation();
            }

            MainState = LayoutState.None;
        }

        #endregion Command Handlers

        #region Methods

        private void UpdateMainPage(string selectedLocality)
        {
            var weatherData = WeatherService.Get5DayForecast(selectedLocality).Result;
            var navPage = App.Current.MainPage as NavigationPage;
            var mainPage = navPage.RootPage as MainPage;
            mainPage.viewModel.ApplyWeatherData(weatherData);
        }

        public async void OnNavigatedTo()
        {
            MainState = LayoutState.Loading;

            await GetPlacemarkAndLocation();

            MainState = LayoutState.None;
        }

        public async Task GetPlacemarkAndLocation()
        {
            try
            {
                Locations.Clear();
                var locations = await WeatherApp.Helpers.Locations.GetLocationsFromStorage();
                locations.ForEach(l => Locations.Add(l));
                Locations.Add(new LocationModel());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion Methods
    }
}