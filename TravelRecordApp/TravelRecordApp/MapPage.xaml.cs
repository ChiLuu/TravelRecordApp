using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private bool hasLocationPermission = false;
        public MapPage()
        {
            InitializeComponent();

            GetPermissions();
        }
        private async void GetPermissions()
        {
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // Prompt the user to turn on in settings
                    // On iOS once a permission has been denied it may not be requested again from the application
                    await DisplayAlert("Location Access", "We need access to your location", "Ok");
                    return;
                }
                if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                {
                    // Prompt the user with additional information as to why the permission is needed
                    await DisplayAlert("Location Access", "We need access to your location", "Ok");
                }

                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Denied)
                    await DisplayAlert("Location denied", "You refused to grant location access so we are unable to display your location.", "Ok");
                else
                    hasLocationPermission = true;
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
            return;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Location change event
            if (hasLocationPermission)
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.PositionChanged += Locator_Position;

                    // Listen for location change with a minimum distance change of 100meters.
                    await locator.StartListeningAsync(TimeSpan.Zero, 100);
                    var position = await locator.GetPositionAsync();
                    MoveMap(position);

                    /*
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        DisplayInMap(posts);
                    }
                    */
                    var posts = await Post.GetUserPosts();
                    DisplayInMap(posts);
                }
                catch(Exception ex)
                {
                    // Unable to get location
                    await DisplayAlert("Unable to get location", ex.Message, "Ok");
                }
            }
        }
        private void DisplayInMap(List<Post> posts)
        {
            locationsMap.Pins.Clear();
            foreach (var post in posts)
            {
                try
                {
                    var position = new Xamarin.Forms.Maps.Position(post.Latitude, post.Longitude);
                    var pin = new Pin()
                    {
                        Type = PinType.SavedPin,
                        Position = position,
                        Label = post.VenueName,
                        Address = post.Address
                    };

                    locationsMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre)
                {
                    DisplayAlert("Unable to get location", nre.Message, "Ok");
                }
                catch (Exception ex)
                {
                    DisplayAlert("Unable to get location", ex.Message, "Ok");
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= Locator_Position;

        }

        private void Locator_Position(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        private void MoveMap(Plugin.Geolocator.Abstractions.Position position)
        {

            // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            var center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var mapSpan = new Xamarin.Forms.Maps.MapSpan(center, 0.1, 0.1);

            locationsMap.MoveToRegion(mapSpan);
        }
    }
}