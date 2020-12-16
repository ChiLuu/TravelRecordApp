using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using TravelRecordApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailPage : ContentPage
    {
        Post selectedPost;
        PostDetailVM viewModel;
        public PostDetailPage()
        {
            
        }
        public PostDetailPage(Post post)
        {
            InitializeComponent();

            viewModel = new PostDetailVM(post);
            BindingContext = viewModel;
        }

        private async void updateButton_Clicked(object sender, EventArgs e)
        {
            selectedPost.Experience = experienceEntry.Text;

            /* local DB with SQLite
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Update(selectedPost);

                if (rows > 0)
                    DisplayAlert("Success", "Experience successfully updated", "Ok");
                else
                    DisplayAlert("Failure", "Experience failed to be updated", "Ok");
            }
            */

            if(await Post.Update(selectedPost))
                await Navigation.PushAsync(new HomePage());
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            /* local DB with SQLite
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Delete(selectedPost);

                if (rows > 0)
                    DisplayAlert("Success", "Experience successfully deleted", "Ok");
                else
                    DisplayAlert("Failure", "Experience failed to be deleted", "Ok");
            }
            */
            if (await Post.Delete(selectedPost))
                await Navigation.PushAsync(new HomePage());
            
        }
    }
}