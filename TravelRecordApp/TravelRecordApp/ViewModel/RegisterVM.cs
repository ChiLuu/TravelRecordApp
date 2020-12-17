using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    public class RegisterVM : INotifyPropertyChanged
    {
        private Users user;

        public Users User 
        {
            get { return user; }
            set 
            { 
                user = value;
                OnPropertyChanged();
            }
        }

        private string email;

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

        private string password;

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

        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged();
                RegisterCommand.ChangeCanExecute();
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set 
            { 
                isBusy = value;
                OnPropertyChanged();
                RegisterCommand.ChangeCanExecute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command RegisterCommand { get; private set; }
        public RegisterVM()
        {
            //  Command for: 
            //      Register Button
            //
            //  On use: 
            //      Attempts to register a new user based on user's input.
            //      If registration is successful then navigate user back to login page.
            //
            //  Restrictions: 
            //      Active if the user name, password, and confirm password field is not empty 
            //      and if current page is not busy (IsBusy).
            RegisterCommand = new Command(
                execute: async () => 
                {
                    IsBusy = true;
                    if (user.Password != ConfirmPassword)
                        await App.Current.MainPage.DisplayAlert("Password Error", "Passwords does not match.", "Ok");
                    else if (await Users.Register(user))
                        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                    IsBusy = false;
                },
                canExecute: () =>
                {
                    if (user != null 
                    && !string.IsNullOrEmpty(user.Password) 
                    && !string.IsNullOrEmpty(ConfirmPassword)
                    && !IsBusy)
                        return true;
                    else
                        return false;
                }
            );
        }
    }
}
