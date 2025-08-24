using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieOrganiser2000.Views;

namespace MovieOrganiser2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AddMoviePage()); // Navigér til din startside
        }
        private void OpenAddMoviePage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddMoviePage());
        }
    }
}