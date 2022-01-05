using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using WeatherApp.ViewModels;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel viewModel => this.BindingContext as MainPageViewModel;

        public MainPage()
        {
            BindingContext = new MainPageViewModel(this);
            InitializeComponent();
        }

        #region Command Handlers

        public void OpenCloseMenu()
        {
            menuView.State = menuView.State.Equals(SideMenuState.LeftMenuShown) ?
                SideMenuState.MainViewShown :
                SideMenuState.LeftMenuShown;
        }

        #endregion Command Handlers
    }
}