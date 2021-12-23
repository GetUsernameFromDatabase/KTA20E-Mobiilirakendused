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
            new MenuItemModel() { Label = "Locations", Icon = "⚙️" },
            new MenuItemModel() { Label = "Settings", Icon = "📍" } ,
        };

        public SideMenuView()
        {
            BindingContext = this;
            SideMenuCommand = new Command<string>(SideMenuCommandHandler);
            InitializeComponent();
        }

        private async void SideMenuCommandHandler(string label)
        {
            Debug.WriteLine(label);
            await Task.Delay(200);
            // Perform navigation
        }
    }
}