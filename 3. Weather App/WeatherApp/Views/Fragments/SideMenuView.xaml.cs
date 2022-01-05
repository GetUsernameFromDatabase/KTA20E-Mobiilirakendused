using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views.Fragments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuView : StackLayout
    {
        public ICommand SideMenuCommand { get; set; }

        public MenuItemModel[] MenuItems => new MenuItemModel[] {
            new MenuItemModel() { Label = "Locations", Icon = "\uf3c5" },
            new MenuItemModel() { Label = "Settings", Icon = "\uf013" } ,
        };

        public SideMenuView()
        {
            BindingContext = this;
            SideMenuCommand = new Command<string>(SideMenuCommandHandler);
            InitializeComponent();
        }

        private async void SideMenuCommandHandler(string label)
        {
            var houser = Parent.Parent.Parent as ContentPage;
            ContentPage newPage = null;
            switch (label)
            {
                case "Locations": newPage = new LocationsPage(); break;
                case "Settings": newPage = new SettingsPage(); break;
            }
            await Navigation.PushAsync(new NavigationPage(newPage));
        }
    }
}