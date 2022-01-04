using System.Diagnostics;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views.Fragments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Weather_DetailedView : Frame
    {
        public DetailedInfo WeatherData
        {
            get { return (DetailedInfo)GetValue(WeatherDataProperty); }
            set { SetValue(WeatherDataProperty, value); }
        }

        public static readonly BindableProperty WeatherDataProperty = BindableProperty.Create(
            nameof(WeatherData), typeof(DetailedInfo), typeof(Weather_DetailedView), null);

        public Weather_DetailedView()
        {
            InitializeComponent();
            this.Content.BindingContext = this;
        }
    }
}