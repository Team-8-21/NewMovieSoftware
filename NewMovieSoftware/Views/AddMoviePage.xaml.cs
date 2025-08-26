using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieOrganiser2000.ViewModels;
using MovieOrganiser2000.Models;
using MovieOrganiser2000.Repositories;

namespace MovieOrganiser2000.Views
{
    /// <summary>
    /// Interaction logic for AddMoviePage.xaml
    /// </summary>
    public partial class AddMoviePage : Page
    {
        public AddMoviePage()
        {
            InitializeComponent();

            var path = @"D:\Programmer\Microsoft Visual Studio\repos\Team821_MovieSoftware\NewMovieSoftware\Data\movies.json";
            DataContext = new AddMovieViewModel(new FileMovieRepository(path));
        }
                

        private void Button_Click_ScheduleShow(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ScheduleShows());
        }
    }
}
