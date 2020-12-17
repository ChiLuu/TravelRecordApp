using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using EmailValidation;
using System.Runtime.CompilerServices;

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
                OnPropertyChanged();
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
                    if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
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
            if (!EmailValidator.Validate(user.Email))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Email", "Please enter a valid Email.", "Ok");
            }
            else
            {
                try
                {
                    var existingUser = await Users.GetUserByEmail(user.Email);

                    if (existingUser == null)
                    {
                        user.password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.password);
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

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
