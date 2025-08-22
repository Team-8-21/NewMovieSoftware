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
        public ICommand CloseTheWindowCommand { get; }

        //Constructor for StartupScreenViewModel

        public StartupScreenViewModel()
        {
            RunTheProgramCommand = new RelayCommand(_ => RunTheProgram());
            CloseTheWindowCommand = new RelayCommand(_ => CloseTheWindow());
        }

        //Til at åbne MainWindow.xaml

        private void RunTheProgram()
        {
            var window = new MainWindow(); 
            window.Show();
            CloseTheWindow(); // Lukker StartupScreen vinduet når MainWindow åbnes
        }

        //Til at lukke StartupScreen vinduet
        private void CloseTheWindow()
            {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is StartupScreen);
            window.Close();
            }
        }
    }
