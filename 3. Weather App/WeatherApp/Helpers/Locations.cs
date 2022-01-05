using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Essentials;

namespace WeatherApp.Helpers
{
    public struct Locations
    {
        public static string locationKey = "locations";

        public static async Task<List<LocationModel>> GetLocationsFromStorage()
        {
            var listLocJson = await SecureStorage.GetAsync(locationKey);
            if (listLocJson == null) return null;

            var locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
            return locations;
        }

        public static Task SaveLocationsToStorage(List<LocationModel> locations)
        {
            return SecureStorage.SetAsync(locationKey, JsonConvert.SerializeObject(locations));
        }

        public static Task SaveLocationsToStorage(ObservableCollection<LocationModel> locations)
        {
            return SecureStorage.SetAsync(locationKey, JsonConvert.SerializeObject(locations));
        }
    }
}