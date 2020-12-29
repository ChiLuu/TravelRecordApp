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
        private readonly PostDetailVM viewModel;
        public PostDetailPage()
        {
            
        }
        public PostDetailPage(Post post)
        {
            InitializeComponent();

            viewModel = new PostDetailVM(post);
            BindingContext = viewModel;
        }
    }
}