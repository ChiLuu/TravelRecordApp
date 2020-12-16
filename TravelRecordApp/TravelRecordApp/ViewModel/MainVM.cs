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
                RegisterNavigationCommand.ChangeCanExecute();
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
            
            //  Command for: 
            //      Register Button
            //
            //  On use: 
            //      Navigate to a new Registration Page. 

            //  Restrictions: 
            //      Only if current page is not busy (IsBusy).
            RegisterNavigationCommand = new Command(
                    async () => await App.Current.MainPage.Navigation.PushAsync(new RegisterPage()), 
                    ()=> !IsBusy);

            //  Command for: 
            //      Login Button
            //
            //  On use: 
            //      Log user in.
            //
            //  Restrictions: 
            //      If user exist, page is not busy, and user had enter an Email and Password.
            LoginCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    bool canLogin = await Users.Login(User.Email, User.Password);
                    if (canLogin)
                        await App.Current.MainPage.Navigation.PushAsync(new HomePage());
                    IsBusy = false;
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
    }
}
