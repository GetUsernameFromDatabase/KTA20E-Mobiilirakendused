using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.ViewModels.Dialogs;
using Xamarin.Forms;
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
            this.BindingContext = new AddLocationDialogViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnDialogOpened();
        }
    }
}