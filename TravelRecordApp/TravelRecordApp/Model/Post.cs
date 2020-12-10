using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TravelRecordApp.Model
{
    public class Post : INotifyPropertyChanged
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

        private string experience;
        public string Experience
        {
            get { return experience; }
            set 
            { 
                experience = value;
                OnPropertyChanged("Experience");
            }
        }

        private string venueName;
        public string VenueName
        {
            get { return venueName; }
            set
            {
                venueName = value;
                OnPropertyChanged("VenueName");
            }
        }

        private string categoryId;
        public string CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
                OnPropertyChanged("CategoryId");
            }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        private int distance;

        public int Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public static async void Insert(Post post)
        {
            await App.MobileService.GetTable<Post>().InsertAsync(post);
        }
        public static async void Update(Post post)
        {
            await App.MobileService.GetTable<Post>().UpdateAsync(post);
        }
        public static async void Delete(Post post)
        {
            await App.MobileService.GetTable<Post>().DeleteAsync(post);
        }

        public static async Task<List<Post>> GetUserPosts()
        {
            return await App.MobileService.GetTable<Post>().Where(p => p.UserId == App.user.Id).ToListAsync();
        }

        public static Dictionary<string, int> PostCategories(List<Post> posts)
        {
            var categories = Post.GetCategoryNamesFromPosts(posts);

            // same as categories
            // var categories2 = posts.OrderBy(p => p.CategoryId).Select(p => p.CategoryName).Distinct().ToList();

            Dictionary<string, int> categoriesCount = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                var count = Post.CountCategoriesInPosts(posts, category);

                // Same as count but different syntax
                //var count2 = posts.Where(p => p.CategoryName == category).ToList().Count;

                categoriesCount.Add(category, count);
            }

            return categoriesCount;
        }
        public static List<string> GetCategoryNamesFromPosts(List<Post> posts)
        {
            return (from p in posts
                    orderby p.CategoryId
                    select p.CategoryName).Distinct().ToList();
        }
        public static int CountCategoriesInPosts(List<Post> posts, string category)
        {
            return (from post in posts
                    where post.CategoryName == category
                    select post).ToList().Count;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
