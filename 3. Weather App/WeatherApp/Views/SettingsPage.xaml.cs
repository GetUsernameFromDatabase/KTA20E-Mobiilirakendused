using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            this.BindingContext = new SettingsPageViewModel(this);
            InitializeComponent();
        }

        public async void BackCommandHandler()
        {
            await Navigation.PopAsync();
        }
    }
}