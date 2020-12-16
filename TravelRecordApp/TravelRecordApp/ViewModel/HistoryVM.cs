using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TravelRecordApp.Model;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModel
{
    class HistoryVM
    {
        public ObservableCollection<Post> Posts { get; set; }

        public Command DeleteCommand { get; private set; }
        public Command NavigateCommand { get; private set; }

        public HistoryVM()
        {
            Posts = new ObservableCollection<Post>();

            DeleteCommand = new Command<Post>(async (Post post) => 
            {
                if (await Post.Delete(post))
                    UpdatePosts();
            });
            NavigateCommand = new Command<Post>(async (Post post) => 
                await App.Current.MainPage.Navigation.PushAsync(new PostDetailPage(post)));
        }
        public async void UpdatePosts()
        {
            Posts.Clear();

            // Get all posts from current user and add each one to Posts which is an ObservableCollection<Post>
            var posts = await Post.GetUserPosts();
            foreach (var post in posts)
                Posts.Add(post);
        }
    }
}
