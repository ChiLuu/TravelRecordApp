using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private Users user;
        private string email;
        private string password;
        private bool isBusy;

        public Users User 
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            } 
        }
        // public LoginCommand LoginCommand { get; set; }
        // public RegisterNavigationCommand RegisterNavigationCommand { get; set; }
        public Command LoginCommand { get; private set; }
        public Command RegisterNavigationCommand { get; private set; }

        public string Email
        {
            get { return email; }
            set 
            { 
                email = value;
                User = new Users()
                {
                    Email = this.Email,
                    Password = this.Password
                };
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set 
            { 
                password = value;
                User = new Users()
                {
                    Email = this.Email,
                    Password = this.Password
                };
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set 
            { 
                isBusy = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainVM()
        {
            User = new Users();
            
            RegisterNavigationCommand = new Command(
                    async () => await App.Current.MainPage.Navigation.PushAsync(new RegisterPage()));

            LoginCommand = new Command(
                execute: () =>
                {
                    Login();
                },
                canExecute: () =>
                {
                    if (user == null || IsBusy)
                        return false;
                    else if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                        return false;
                    else
                        return true;
                });
        }

        public async void Login()
        {
            IsBusy = true;
            bool canLogin = await Users.Login(User.Email, User.Password);
            if (canLogin)
                await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            else
                await App.Current.MainPage.DisplayAlert("Error", "Email or password are incorrect", "Ok");

            IsBusy = false;
        }

        //public async void Navigate()
        //{
        //    await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        //}
    }
}
