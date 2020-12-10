using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        Users user;
        public RegisterPage()
        {
            InitializeComponent();

            user = new Users();
            containerStackLayout.BindingContext = user;
        }

        private async void registerButton_Clicked(object sender, EventArgs e)
        {
            if(user.Password == confirmPasswordEntry.Text)
            {
                var existingUser = await Users.GetUserByEmail(user.Email);

                if (existingUser == null)
                {
                    Users.Register(user);
                    await DisplayAlert("Registered!", "Your account has been registered successfully.", "Ok");
                    await Navigation.PushAsync(new MainPage());
                }
                else
                    await DisplayAlert("User Exist", "The email is already registered.", "Ok");
            }
            else
                await DisplayAlert("Password Error", "Passwords does not match.", "Ok");
            
        }
    }
}