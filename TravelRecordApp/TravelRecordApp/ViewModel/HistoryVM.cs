using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TravelRecordApp.Model;

namespace TravelRecordApp.ViewModel
{
    class HistoryVM
    {
        public ObservableCollection<Post> Posts { get; set; }

        public HistoryVM()
        {
            Posts = new ObservableCollection<Post>();
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
