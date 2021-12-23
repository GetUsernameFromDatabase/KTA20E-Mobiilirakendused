using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeatherApp.Models;
using System;

namespace WeatherApp.Views.Fragments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuView : StackLayout
    {
        public MenuItemModel[] MenuItems => new MenuItemModel[] {
            new MenuItemModel() { Label = "Locations", Icon = "⚙️" },
            new MenuItemModel() { Label = "Settings", Icon = "📍" } ,
        };

        public SideMenuView()
        {
            BindingContext = this;
            InitializeComponent();
        }
    }
}