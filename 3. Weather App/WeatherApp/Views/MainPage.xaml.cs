using WeatherApp.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
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