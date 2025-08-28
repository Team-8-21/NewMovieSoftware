using MovieOrganiser2000.Models;
using MovieOrganiser2000.Helpers;
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
using System.Windows.Shapes;
using IOPath = System.IO.Path;
using Path = System.IO.Path;

namespace MovieOrganiser2000.Views
{
    /// <summary>
    /// Interaction logic for ScheduleShows.xaml
    /// </summary>
    public partial class ScheduleShows : Page
    {
        public ScheduleShows()
        {
            InitializeComponent();

            var theaterService = new TheaterService();

            // 1) AppData rod (vælg LocalApplicationData eller ApplicationData)
            var appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // var appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // Roaming

            // 2) App-specifik mappe + fil
            var appDir = Path.Combine(appDataRoot, "MovieOrganiser2000");
            Directory.CreateDirectory(appDir);
            var moviesPath = Path.Combine(appDir, "movies.json");

            // 3) Seed første gang fra projektets Data\movies.json
            if (!File.Exists(moviesPath))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;               // bin\Debug\...
                var seedPath = Path.Combine(baseDir, "Data", "movies.json");       // husk at sætte Copy to Output Directory på seed-filen
                if (File.Exists(seedPath))
                {
                    File.Copy(seedPath, moviesPath, overwrite: false);
                }
                else
                {
                    // fallback hvis seed ikke findes
                    File.WriteAllText(moviesPath, "[]");
                }
            }

            // 4) Brug AppData-filen i dit repository
            var movieRepo = new FileMovieRepository(moviesPath);

            // Tilføj Schedule-repo
            var schedulesPath = Path.Combine(appDir, "showschedules.json");

            if (!File.Exists(schedulesPath))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var seedPath = Path.Combine(baseDir, "Data", "showschedules.json");
                if (File.Exists(seedPath))
                    File.Copy(seedPath, schedulesPath, overwrite: false);
                else
                    File.WriteAllText(schedulesPath, "[]");
            }

            var scheduleRepo = new FileScheduleRepository(schedulesPath);

            DataContext = new ScheduleShowViewModel(theaterService, movieRepo, scheduleRepo);
        }
        private void Button_Click_AddMoviePage (object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddMoviePage());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
