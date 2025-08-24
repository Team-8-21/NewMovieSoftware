using MovieOrganiser2000.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieOrganiser2000.ViewModels
{

    //Relay-kommando til at åbne MainWindow.xaml fra Startupscreen. 
    
    internal class StartupScreenViewModel
    {
            public ICommand RunTheProgramCommand { get; }

            public StartupScreenViewModel()
            {
                RunTheProgramCommand = new RelayCommand(_ => RunTheProgram());
            }

            private void RunTheProgram()
            {
                var window = new MainWindow(); // Dit nye vindue
                window.Show();
            }
        }
    }
