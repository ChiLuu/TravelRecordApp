using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TravelRecordApp.Model
{
    public class Users : INotifyPropertyChanged
    {
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public static async Task<Users> GetUserByEmail(string email)
        {
            return (await App.MobileService.GetTable<Users>().Where(u => u.Email == email).ToListAsync()).FirstOrDefault();
        }

        public static async Task<bool> Login(string email, string password)
        {
            bool isEmailEmpty = string.IsNullOrEmpty(email);
            bool isPasswordEmpty = string.IsNullOrEmpty(password);

            if (isEmailEmpty || isPasswordEmpty)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email or password was not entered.", "Ok");
                return false;
            }
            else
            {
                try
                {
                    var user = await Users.GetUserByEmail(email);
                    if (user != null && user.Password == password)
                    {
                        App.user = user;
                        return true;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Email or password are incorrect", "Ok");
                        return false;
                    }
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }
        }
        public static async Task<bool> Register(Users user)
        {
            if (user.Password != user.ConfirmPassword)
                await App.Current.MainPage.DisplayAlert("Password Error", "Passwords does not match.", "Ok");
            else
            {
                try
                {
                    var existingUser = await Users.GetUserByEmail(user.Email);

                    if (existingUser == null)
                    {
                        // Get User table from web service and add user
                        await App.MobileService.GetTable<Users>().InsertAsync(user);
                        await App.Current.MainPage.DisplayAlert("Registered!", "Your account has been registered successfully.", "Ok");
                        return true;
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("User Exist", "The email is already registered.", "Ok");
                        
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Registration Error", "Something went wrong.", "Ok");
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }
            return false;

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
