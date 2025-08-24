using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;


namespace MovieOrganiser2000.Models
{
    public class MovieTheater
    {

        public int _id;
        public string _name;
        
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<MovieScreen> Screens { get; set; }

        public MovieTheater(int id, string name, string v)
        {
            Id = id;
            Name = name;
            Screens = new ObservableCollection<MovieScreen>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
