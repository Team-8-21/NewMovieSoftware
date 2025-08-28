using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieOrganiser2000.Helpers;
using MovieOrganiser2000.Models;
using MovieOrganiser2000.Repositories;
using ScheduleOrganiser2000.Repositories;

namespace MovieOrganiser2000.ViewModels
{
    public class ScheduleShowViewModel : INotifyPropertyChanged
    {
        private readonly IMovieTheater _movieTheaterService;
        private readonly IMovieRepository _movieRepository;
        private readonly IScheduleRepository _scheduleRepository;

        private MovieTheater _selectedTheater;
        private MovieScreen _selectedScreen;
        private Movie _selectedMovie;
        private DateTime? _selectedDate;
        private bool _isLoading;
        private int _movieLength;
        private string _selectedTime;
        private string schedulesPath;

        public RelayCommand AddScheduleCommand { get; private set; }
        // public ObservableCollection<string> AvailableTimes { get; private set; };

        public event PropertyChangedEventHandler? PropertyChanged;

        public ScheduleShowViewModel(IMovieTheater movieTheaterService,
                                     IMovieRepository movieRepository,
                                     IScheduleRepository scheduleRepository)
        {
            _movieTheaterService = movieTheaterService;
            _movieRepository = movieRepository;
            //_scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
            //_repo = repo ?? throw new ArgumentNullException(nameof(repo));

            Theaters = new ObservableCollection<MovieTheater>();
            AvailableScreens = new ObservableCollection<MovieScreen>();
            Movies = new ObservableCollection<Movie>();
            AvailableTimes = new ObservableCollection<string>();

            LoadTheatersCommand = new RelayCommand(async _ => await LoadTheatersAsync());
            ScheduleShowCommand = new RelayCommand(async _ => await ScheduleShowAsync(), _ => CanScheduleShow());
            AddScheduleCommand = new RelayCommand(_ => AddShow(),_ => CanScheduleShow());

            _ = LoadTheatersAsync();
            _ = LoadMoviesAsync();
            LoadTimes();
        }

        // --- BINDINGS TIL XAML ---
        public ObservableCollection<MovieTheater> Theaters { get; }
        public ObservableCollection<MovieScreen> AvailableScreens { get; }
        public ObservableCollection<Movie> Movies { get; }
        public ObservableCollection<string> AvailableTimes { get; private set; }

        public MovieTheater SelectedTheater
        {
            get => _selectedTheater;
            set
            {
                if (SetProperty(ref _selectedTheater, value, nameof(SelectedTheater)))
                    _ = LoadScreensForSelectedTheaterAsync();
            }
        }

        public string SelectedTime
        {
            get => _selectedTime;
            set => SetProperty(ref _selectedTime, value, nameof(SelectedTime));
        }

        public MovieScreen SelectedScreen
        {
            get => _selectedScreen;
            set => SetProperty(ref _selectedScreen, value, nameof(SelectedScreen));
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                if (SetProperty(ref _selectedMovie, value, nameof(SelectedMovie)))
                {
                    // Opdatér visning/tekstfelt til længde når film skiftes
                    MovieLength = _selectedMovie?.MovieLength ?? 0;
                }
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value, nameof(SelectedDate));
        }

        public int MovieLength
        {
            get => _movieLength;
            set => SetProperty(ref _movieLength, value, nameof(MovieLength));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(IsLoading));
        }

        public ICommand LoadTheatersCommand { get; }
        public ICommand ScheduleShowCommand { get; }
        //public ICommand AddShowCommand { get; }
        // public object AddSchedule { get; private set; }

        // --- LOAD DATA ---
        private async Task LoadTheatersAsync()
        {
            IsLoading = true;
            try
            {
                var theaters = await _movieTheaterService.GetAllTheatersAsync();
                Theaters.Clear();
                foreach (var t in theaters) Theaters.Add(t);
            }
            finally { IsLoading = false; }
        }

        private void LoadTimes()
        {
            AvailableTimes.Clear();

            // Halvtimes-slots 10:00–23:00 (ret efter behov)
            for (int hour = 10; hour <= 23; hour++)
            {
                AvailableTimes.Add($"{hour:00}:00");
                AvailableTimes.Add($"{hour:00}:30");
            }

            // Standardvalg
            if (string.IsNullOrEmpty(SelectedTime))
                SelectedTime = "19:00";
        }

        private async Task LoadScreensForSelectedTheaterAsync()
        {
            if (SelectedTheater == null) return;

            IsLoading = true;
            try
            {
                var screens = await _movieTheaterService.GetAvailableScreensForTheaterAsync(SelectedTheater.Id);
                AvailableScreens.Clear();
                foreach (var s in screens) AvailableScreens.Add(s);
                SelectedScreen = null;
            }
            finally { IsLoading = false; }
        }

        private Task LoadMoviesAsync()
        {
            var all = _movieRepository.GetAll().ToList();
            System.Diagnostics.Debug.WriteLine($"[Schedule] Loaded {all.Count} movies");

            Movies.Clear();
            foreach (var m in all)
            {
                System.Diagnostics.Debug.WriteLine($"[Schedule] Movie: {m.Title} ({m.MovieLength} min)");
                Movies.Add(m);
            }
            return Task.CompletedTask;
        }
        


        // --- COMMAND LOGIK ---
        private bool CanScheduleShow()
        {
            return SelectedTheater != null
                && SelectedScreen != null
                && SelectedMovie != null
                && SelectedDate != null
                && !string.IsNullOrEmpty(SelectedTime)
                && !IsLoading;
        }

        private async Task ScheduleShowAsync()
        {
            /*if (!CanScheduleShow()) return;

            // TODO: Lav et Show-objekt og gem det, hvis du har et ShowRepository.
            // Fx:
            // var show = new Show(SelectedMovie, SelectedTheater, SelectedScreen, SelectedDate.Value);
            // _showRepository.Add(show);

            // Evt. markér skærm som optaget og refresh
            SelectedScreen.IsAvailable = false;
            await LoadScreensForSelectedTheaterAsync();
            
            if (!CanScheduleShow()) return;

            try
            {
                // Parse the selected time
                if (!TimeOnly.TryParse(SelectedTime, out var showTime))
                {
                    System.Diagnostics.Debug.WriteLine("Invalid time format");
                    return;
                }

                // Create a new schedule
                var schedule = new Schedule
                {
                    ScheduleId = new Random().Next(1000, 9999), // Generate a random ID
                    Movie = SelectedMovie,
                    Date = DateOnly.FromDateTime(SelectedDate.Value),
                    Time = showTime,
                    Theater = SelectedTheater.Name,
                    Screen = SelectedScreen.Name
                };

                // Save to repository
                _scheduleRepository.AddSchedule(schedule);

                System.Diagnostics.Debug.WriteLine($"Schedule created: {SelectedMovie.Title} at {SelectedTheater.Name} - {SelectedScreen.Name} on {SelectedDate:yyyy-MM-dd} at {SelectedTime}");

                // Mark screen as unavailable (optional)
                SelectedScreen.IsAvailable = false;
                await LoadScreensForSelectedTheaterAsync();

                // Clear selections
                ClearSelections();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scheduling show: {ex.Message}");
            }
            */

            //DEBUG
            System.Diagnostics.Debug.WriteLine("=== ScheduleShowAsync START ===");
            System.Diagnostics.Debug.WriteLine($"CanScheduleShow: {CanScheduleShow()}");
            System.Diagnostics.Debug.WriteLine($"SelectedTheater: {SelectedTheater?.Name}");
            System.Diagnostics.Debug.WriteLine($"SelectedScreen: {SelectedScreen?.Name}");
            System.Diagnostics.Debug.WriteLine($"SelectedMovie: {SelectedMovie?.Title}");
            System.Diagnostics.Debug.WriteLine($"SelectedDate: {SelectedDate}");
            System.Diagnostics.Debug.WriteLine($"SelectedTime: {SelectedTime}");

            if (!CanScheduleShow())
            {
                System.Diagnostics.Debug.WriteLine("CanScheduleShow returned false - EXITING");
                return;
            }

            try
            {
                // Parse the selected time
                if (!TimeOnly.TryParse(SelectedTime, out var showTime))
                {
                    System.Diagnostics.Debug.WriteLine("Invalid time format - EXITING");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Parsed time successfully: {showTime}");

                // Create a new schedule
                var schedule = new Schedule
                {
                    ScheduleId = new Random().Next(1000, 9999),
                    Movie = SelectedMovie,
                    Date = DateOnly.FromDateTime(SelectedDate.Value),
                    Time = showTime,
                    Theater = SelectedTheater.Name,
                    Screen = SelectedScreen.Name
                };

                System.Diagnostics.Debug.WriteLine($"Created schedule object: ID={schedule.ScheduleId}");

                // Save to repository
                System.Diagnostics.Debug.WriteLine("About to call _scheduleRepository.AddSchedule()");
                _scheduleRepository.AddSchedule(schedule);
                System.Diagnostics.Debug.WriteLine("AddSchedule() completed successfully");

                // Clear selections
                ClearSelections();
                System.Diagnostics.Debug.WriteLine("=== ScheduleShowAsync END ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in ScheduleShowAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        

        private void AddShow()
        {
            /*
            InitializeComponent();

            // Vælg Local eller Roaming
            var appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDir = Path.Combine(appDataRoot, "MovieOrganiser2000");
            Directory.CreateDirectory(appDir);

            schedulesPath = Path.Combine(appDir, "showschedules.json");

            // Seed første gang fra projektets Data\showschedules.json
            if (!File.Exists(schedulesPath))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var seedPath = Path.Combine(baseDir, "Data", "showschedules.json");
                if (File.Exists(seedPath))
                    File.Copy(seedPath, schedulesPath, overwrite: false);
                else
                    File.WriteAllText(schedulesPath, "[]");
            }

            var repo = new FileScheduleRepository(schedulesPath);
            // OBS: Du kan her åbne en ny View med AddScheduleViewModel(repo), hvis nødvendigt.
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
            */
            _ = ScheduleShowAsync();
        }

        private void ClearSelections()
        {
            SelectedMovie = null;
            SelectedScreen = null;
            SelectedDate = null;
            SelectedTime = "19:00";
        }


        // --- Helper ---
        protected bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}