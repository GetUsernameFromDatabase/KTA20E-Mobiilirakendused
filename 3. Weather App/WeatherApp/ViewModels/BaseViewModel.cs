using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties

        public event PropertyChangedEventHandler PropertyChanged; // Managed by PropertyChanged.Fody

        public string Title { get; set; }
        public LayoutState MainState { get; set; }
        public bool HasNoInternetConnection { get; set; }

        #endregion Properties

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
            HasNoInternetConnection = !Connectivity.NetworkAccess.Equals(NetworkAccess.Internet);
        }

        #region Internet Connection

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            HasNoInternetConnection = !e.NetworkAccess.Equals(NetworkAccess.Internet);
        }

        #endregion Internet Connection
    }
}