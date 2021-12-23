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

        public Dictionary<string, ICommand> Commands { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }
        public LayoutState MainState { get; set; }
        public bool HasNoInternetConnection { get; set; }

        #endregion Properties

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
            HasNoInternetConnection = !Connectivity.NetworkAccess.Equals(NetworkAccess.Internet);
            Commands = new Dictionary<string, ICommand>();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Internet Connection

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            HasNoInternetConnection = !e.NetworkAccess.Equals(NetworkAccess.Internet);
        }

        #endregion Internet Connection

        #region IPageLifecycleAware

        public virtual void OnAppearing()
        { }

        public virtual void OnDisappearing()
        { }

        #endregion IPageLifecycleAware
    }
}