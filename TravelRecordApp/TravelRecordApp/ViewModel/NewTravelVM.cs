using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TravelRecordApp.Model;
using TravelRecordApp.ViewModel.Commands;

namespace TravelRecordApp.ViewModel
{
    public class NewTravelVM : INotifyPropertyChanged
    {
        public PostCommand PostCommand { get; set; }


        private Post post;

        public Post Post
        {
            get { return post; }
            set 
            { 
                post = value;
                OnPropertyChanged("Post");
            }
        }

        private string experience;

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
                OnPropertyChanged("Experience");
            }
        }

        private Venue venue;

        public Venue Venue
        {
            get { return venue; }
            set 
            { 
                venue = value;
                Post = new Post()
                {
                    Experience = this.Experience,
                    Venue = this.Venue
                };
                OnPropertyChanged("Venue");
            }
        }

        public NewTravelVM()
        {
            PostCommand = new PostCommand(this);
            Post = new Post();
            Venue = new Venue();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void PublishPost(Post post)
        {
            try
            {
                /* This is for local DB using SQLite.
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    int rows = conn.Insert(post);

                    if (rows > 0)
                        DisplayAlert("Success", "Experience successfully inserted", "Ok");
                    else
                        DisplayAlert("Failure", "Experience failed to be inserted", "Ok");
                }*/

                if(await Post.Insert(post))
                    await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            catch (NullReferenceException nre)
            {
                await App.Current.MainPage.DisplayAlert("Failure", "Unable to add your Post", "Ok");
                Console.WriteLine(nre.Message + "\n" + nre.StackTrace);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failure", "Unable to add your new Post", "Ok");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
