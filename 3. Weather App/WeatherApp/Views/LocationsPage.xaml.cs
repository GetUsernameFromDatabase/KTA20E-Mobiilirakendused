using System;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationsPage : ContentPage
    {
        private LocationsPageViewModel viewModel => this.BindingContext as LocationsPageViewModel;

        public LocationsPage()
        {
            this.BindingContext = new LocationsPageViewModel(this);
            InitializeComponent();
        }

        public async void BackCommandHandler()
        {
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnNavigatedTo();
        }
    }
}