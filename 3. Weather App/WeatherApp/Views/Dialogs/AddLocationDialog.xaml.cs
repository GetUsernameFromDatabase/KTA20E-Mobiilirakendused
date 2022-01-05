using Rg.Plugins.Popup.Pages;
using WeatherApp.ViewModels.Dialogs;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLocationDialog : PopupPage
    {
        private AddLocationDialogViewModel viewModel => this.BindingContext as AddLocationDialogViewModel;

        public AddLocationDialog()
        {
            InitializeComponent();
            this.BindingContext = new AddLocationDialogViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnDialogOpened();
        }
    }
}