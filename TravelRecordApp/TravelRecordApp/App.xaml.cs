using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    public partial class App : Application
    {
        // Locally stored SQLite DB as determine by platform specific directory path.
        public static string DatabaseLocation = string.Empty;

        // Azure Mobile Service Client's URL.
        public static MobileServiceClient MobileService = new MobileServiceClient(Constants.APP_SERVICE_URL);

        // Current logged in user.
        public static Users user = new Users();

        // Locally stored data of user(s)'s posts.
        public static IMobileServiceSyncTable<Post> postsTable;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public App(string databaseLocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            DatabaseLocation = databaseLocation;
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
