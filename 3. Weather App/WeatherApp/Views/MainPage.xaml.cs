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
        public MainPage()
        {
            BindingContext = new MainPageViewModel(this);
            InitializeComponent();
            ShowLocationDialogueIfNeeded();
        }

        private async void ShowLocationDialogueIfNeeded()
        {
            var locations = await SecureStorage.GetAsync("locations");
            if (locations == null)
            {
                await PopupNavigation.Instance.PushAsync(new AddLocationDialog());
            }
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