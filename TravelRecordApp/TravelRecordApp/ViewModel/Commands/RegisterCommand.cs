using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelRecordApp.Model;

namespace TravelRecordApp.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public RegisterVM RegisterViewModel { get; set; }

        public RegisterCommand(RegisterVM registerVM)
        {
            RegisterViewModel = registerVM;
        }

        public bool CanExecute(object parameter)
        {
            var user = (Users)parameter;
            if (user != null && !string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.ConfirmPassword))
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            var user = (Users)parameter;
            RegisterViewModel.Register(user);
        }
    }
}
