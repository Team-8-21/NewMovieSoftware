using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieOrganiser2000.Helpers;
using MovieOrganiser2000.Models;
using MovieOrganiser2000.Repositories;

namespace MovieOrganiser2000.ViewModels
{
    public class ScheduleShowViewModel : INotifyPropertyChanged
    {
        private readonly IMovieTheater _movieTheaterService;
        private MovieTheater _selectedTheater;
        private MovieScreen _selectedScreen;
        private bool _isLoading;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ScheduleShowViewModel(IMovieTheater movieTheaterService)
        {
            _movieTheaterService = movieTheaterService;
            Theaters = new ObservableCollection<MovieTheater>();
            AvailableScreens = new ObservableCollection<MovieScreen>();

            LoadTheatersCommand = new RelayCommand(async _ => await LoadTheatersAsync());
            ScheduleShowCommand = new RelayCommand(async _ => await ScheduleShowAsync(), _ => CanScheduleShow());

            _ = LoadTheatersAsync();
        }

        public ObservableCollection<MovieTheater> Theaters { get; }
        public ObservableCollection<MovieScreen> AvailableScreens { get; }

        public MovieTheater SelectedTheater
        {
            get => _selectedTheater;
            set
            {
                if (SetProperty(ref _selectedTheater, value, nameof(SelectedTheater)))
                {
                    _ = LoadScreensForSelectedTheaterAsync();
                }
            }
        }

        public MovieScreen SelectedScreen
        {
            get => _selectedScreen;
            set => SetProperty(ref _selectedScreen, value, nameof(SelectedScreen));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(IsLoading));
        }

        public ICommand LoadTheatersCommand { get; }
        public ICommand ScheduleShowCommand { get; }

        private async Task LoadTheatersAsync()
        {
            IsLoading = true;
            try
            {
                var theaters = await _movieTheaterService.GetAllTheatersAsync();
                Theaters.Clear();
                foreach (var theater in theaters)
                {
                    Theaters.Add(theater);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadScreensForSelectedTheaterAsync()
        {
            if (SelectedTheater == null) return;

            IsLoading = true;
            try
            {
                var screens = await _movieTheaterService.GetAvailableScreensForTheaterAsync(SelectedTheater.Id);
                AvailableScreens.Clear();
                foreach (var screen in screens)
                {
                    AvailableScreens.Add(screen);
                }

                // Reset selected screen
                SelectedScreen = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanScheduleShow()
        {
            return SelectedTheater != null && SelectedScreen != null && !IsLoading;
        }

        private async Task ScheduleShowAsync()
        {
            if (!CanScheduleShow()) return;

            // Implement your show scheduling logic here
            // For example, mark the screen as unavailable
            SelectedScreen.IsAvailable = false;

            // Optionally, refresh the available screens
            await LoadScreensForSelectedTheaterAsync();
        }

        // GENERIC SetProperty helper
        protected bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
