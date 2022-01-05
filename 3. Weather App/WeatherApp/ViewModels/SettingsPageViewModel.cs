using System;
using WeatherApp.Models.Weather;
using WeatherApp.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Properties

        public string Units { get; set; }

        #endregion Properties

        #region Commands

        public Command BackCommand { get; set; }
        public Command MetricCommand { get; set; }
        public Command ImperialCommand { get; set; }

        #endregion Commands

        #region Constructors

        public SettingsPageViewModel(SettingsPage SettingsPage)
        {
            BackCommand = new Command(SettingsPage.BackCommandHandler);
            MetricCommand = new Command(MetricCommandHandler);
            ImperialCommand = new Command(ImperialCommandHandler);
        }

        public SettingsPageViewModel()
        { throw new NotImplementedException("Not meant to be used"); }

        #endregion Constructors

        #region Command Handlers

        private void MetricCommandHandler()
        {
            Units = "metric";
            UnitChanged();
        }

        private void ImperialCommandHandler()
        {
            Units = "imperial";
            UnitChanged();
        }

        #endregion Command Handlers

        private void UnitChanged()
        {
            Preferences.Set("units", Units);
            var navPage = App.Current.MainPage as NavigationPage;
            var mainPage = navPage.RootPage as MainPage;
            mainPage.viewModel.ConvertToPreferredUnits();
        }

        public void OnNavigatedTo()
        {
            MainState = LayoutState.Loading;

            Units = Preferences.Get("units", "metric");

            MainState = LayoutState.None;
        }
    }
}