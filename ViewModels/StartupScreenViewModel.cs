using MovieOrganiser2000.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MovieOrganiser2000.ViewModels
{

    //Relay-kommando til at åbne MainWindow.xaml fra Startupscreen. 

    internal class StartupScreenViewModel
    {

        //Definerer kommandoer til XAML binding

        public ICommand RunTheProgramCommand { get; }
        public ICommand CloseTheStartupScreenCommand { get; }

        //Constructor for StartupScreenViewModel

        public StartupScreenViewModel()
        {
            RunTheProgramCommand = new RelayCommand(_ => RunTheProgram());
            CloseTheStartupScreenCommand = new RelayCommand(_ => CloseTheStartupScreen());
        }

        //Til at åbne MainWindow.xaml

        private void RunTheProgram()
        {
            var window = new MainWindow(); 
            window.Show();
            CloseTheStartupScreen(); // Lukker StartupScreen vinduet når MainWindow åbnes
        }

        //Til at lukke StartupScreen vinduet
        private void CloseTheStartupScreen()
            {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is StartupScreen);
            window.Close();
            }
        }
    }
