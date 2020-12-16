using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    public class HomeVM
    {
        public Command NavCommand { get; private set; }

        public HomeVM()
        {
            NavCommand = new Command(async () => await App.Current.MainPage.Navigation.PushAsync(new NewTravelPage()));
        }
    }
}
