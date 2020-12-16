using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    public class NewTravelVM : INotifyPropertyChanged
    {
        private Post post;
        private Venue venue;
        private string experience;
        private bool isBusy;
        public Command PostCommand { get; private set; }

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
                Post = new Post()
                {
                    Experience = this.Experience,
                    Venue = this.Venue
                };
                OnPropertyChanged();
            }
        }
        public Venue Venue
        {
            get { return venue; }
            set 
            { 
                venue = value;
                if(Post != null)
                {
                    Post = new Post()
                    {
                        Experience = this.Experience,
                        Venue = this.Venue
                    };
                }
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
                PostCommand.ChangeCanExecute();
            }
        }
        public NewTravelVM()
        {
            PostCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    if (await Post.Insert(post))
                        await App.Current.MainPage.Navigation.PushAsync(new HomePage());
                    IsBusy = false;
                },
                canExecute: () =>
                {
                    if (post != null 
                    && !string.IsNullOrEmpty(post.Experience) 
                    && post.Venue != null
                    && !IsBusy)
                        return true;
                    else
                        return false;
                });
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
