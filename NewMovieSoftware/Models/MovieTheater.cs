using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;


namespace MovieOrganiser2000.Models
{
    public class MovieTheater
    {

        public int _id;
        public string _name;
        private string _location;

        public int Id
        {
            get => _id;
            set 
            { 
                if (_id == value) return; 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            }
        }

        public string Name
        {
            get => _name;
            set 
            { 
                if (_name == value) return; 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            }
        }

        public string Location
        {
            get => _location;
            set { 
                if (_location == value) return; 
                _location = value; 
                OnPropertyChanged(nameof(Location)); 
            }
        }
        public ObservableCollection<MovieScreen> Screens { get; } = new();

        public MovieTheater(int id, string name, string location)
        {
            Id = id;
            Name = name;
            _location = location ?? string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        
    }
}
