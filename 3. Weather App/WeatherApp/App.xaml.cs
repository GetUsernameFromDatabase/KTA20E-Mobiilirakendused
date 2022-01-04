using WeatherApp.Views;
using WeatherApp.Views.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ExportFont("FontAwesome.ttf", Alias = "FontAwesome")]

namespace WeatherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}