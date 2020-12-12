using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TravelRecordApp.ViewModel.Commands
{
    public class RegisterNavigationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public MainVM mainViewModel { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public RegisterNavigationCommand(MainVM mainVM)
        {
            mainViewModel = mainVM;
        }

        public void Execute(object parameter)
        {
            mainViewModel.Navigate();
        }
    }
}
