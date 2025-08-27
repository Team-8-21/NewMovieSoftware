using MovieOrganiser2000.Models;
using MovieOrganiser2000.Repositories;
using MovieOrganiser2000.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

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

            // Vælg Local eller Roaming
            var appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDir = Path.Combine(appDataRoot, "MovieOrganiser2000");
            Directory.CreateDirectory(appDir);

            var moviesPath = Path.Combine(appDir, "movies.json");

            // Seed første gang fra projektets Data\movies.json (samme som ScheduleShows)
            if (!File.Exists(moviesPath))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;   // bin\...\ 
                var seedPath = Path.Combine(baseDir, "Data", "movies.json");
                if (File.Exists(seedPath))
                    File.Copy(seedPath, moviesPath, overwrite: false);
                else
                    File.WriteAllText(moviesPath, "[]");
            }

            var repo = new FileMovieRepository(moviesPath);
            DataContext = new AddMovieViewModel(repo);
        }


        private void Button_Click_ScheduleShow(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ScheduleShows());
        }
    }
}
