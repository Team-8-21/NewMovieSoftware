using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class MovieScreen
    {
        public int _id;
        public string _name;
        private bool _isAvailable;
        private int _seatingCapacity;

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

        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                _isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public int SeatingCapacity
        {
            get => _seatingCapacity;
            set
            {
                _seatingCapacity = value;
                OnPropertyChanged(nameof(SeatingCapacity));
            }
        }

        public MovieScreen(int id, string name, int seatingCapacity, bool isAvailable = true)
        {
            Id = id;
            Name = name;
            SeatingCapacity = seatingCapacity;
            IsAvailable = isAvailable;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
