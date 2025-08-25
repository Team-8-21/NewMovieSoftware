using MovieOrganiser2000.Helpers;
using MovieOrganiser2000.Models;
using MovieSoftware.MVVM.Model.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieOrganiser2000.ViewModels
{
    public class AddMovieViewModel : INotifyPropertyChanged
    {
        private Genre _selectedGenre;
        public ObservableCollection<Genre> Genres { get; }
        public User CurrentUser { get; set; }

        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (_selectedGenre != value)
                {
                    _selectedGenre = value;
                    OnPropertyChanged(nameof(SelectedGenre));
                }
            }
        }

        private string _title = "";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private int _movieLength;
        public int MovieLength
        {
            get => _movieLength;
            set
            {
                _movieLength = value;
                OnPropertyChanged(nameof(MovieLength));
            }
        }

        private string _director = "";
        public string Director
        {
            get => _director;
            set
            {
                _director = value;
                OnPropertyChanged(nameof(Director));
            }
        }

        private DateOnly _premiere;
        public DateOnly Premiere
        {
            get => _premiere;
            set
            {
                _premiere = value;
                OnPropertyChanged(nameof(Premiere));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddMovieCommand { get; }

        private MovieManager _movieManager = new MovieManager(); // Eller inject senere

        public AddMovieViewModel()
        {
            Genres = new ObservableCollection<Genre>((Genre[])Enum.GetValues(typeof(Genre)));
            SelectedGenre = Genre.Ukendt;

            AddMovieCommand = new RelayCommand(_ => AddMovie());
        }

        private void AddMovie()
        {
            var movie = new Movie(Title, MovieLength, SelectedGenre, Director, Premiere);
            _movieManager.AddMovie(movie);

            Title = "";
            MovieLength = 0;
            SelectedGenre = Genre.Ukendt;
            Director = "";
            Premiere = default;
        }

    }
}
