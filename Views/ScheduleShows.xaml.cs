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
using System.Windows.Shapes;
using MovieOrganiser2000.ViewModels;
using MovieOrganiser2000.Models;

namespace MovieOrganiser2000.Views
{
    /// <summary>
    /// Interaction logic for ScheduleShows.xaml
    /// </summary>
    public partial class ScheduleShows : Window
    {
        public ScheduleShows()
        {
            InitializeComponent();

            var _movieTheaterService = new TheaterService();
            var viewModel = new ScheduleShowViewModel(_movieTheaterService);

            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
