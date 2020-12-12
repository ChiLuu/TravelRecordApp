using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TravelRecordApp.Model;

namespace TravelRecordApp.ViewModel.Commands
{
    public class PostCommand : ICommand
    {
        NewTravelVM viewModel;

        public PostCommand(NewTravelVM newTravelVM)
        {
            viewModel = newTravelVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var post = (Post)parameter;

            if (post != null && !string.IsNullOrEmpty(post.Experience) && post.Venue != null)
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            var post = (Post)parameter;
            viewModel.PublishPost(post);
        }
    }
}
