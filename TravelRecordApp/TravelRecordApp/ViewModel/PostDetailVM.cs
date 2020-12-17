using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    public class PostDetailVM : INotifyPropertyChanged
    {
        private Post post;
        private string experience;
        private bool isBusy;
        public Command DeleteCommand { get; private set; }
        public Command UpdateCommand { get; private set; }

        
        public Post Post
        {
            get { return post; }
            set 
            { 
                post = value;
                OnPropertyChanged();
            }
        }
        public string Experience
        {
            get { return experience; }
            set 
            { 
                experience = value;
                Post.Experience = Experience;
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
            }
        }
        public PostDetailVM(Post selectedPost)
        {
            post = selectedPost;
            Experience = post.Experience;

            DeleteCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    if (await Post.Delete(post))
                        await App.Current.MainPage.Navigation.PushAsync(new HomePage());
                    IsBusy = false;
                },
                canExecute: () => !IsBusy);

            UpdateCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    if (await Post.Update(post))
                        await App.Current.MainPage.Navigation.PushAsync(new HomePage());
                    IsBusy = false;
                },
                canExecute: () => !IsBusy);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
