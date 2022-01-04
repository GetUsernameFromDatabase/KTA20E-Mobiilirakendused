using System;
using WeatherApp.Views;
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
            Preferences.Set("units", Units);
        }

        private void ImperialCommandHandler()
        {
            Units = "imperial";
            Preferences.Set("units", Units);
        }

        #endregion Command Handlers
    }
}