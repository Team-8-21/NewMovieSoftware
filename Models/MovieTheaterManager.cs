using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieOrganiser2000.Repositories;

namespace MovieOrganiser2000.Models
{
    public class TheaterService : IMovieTheater
    {
        private readonly List<MovieTheater> _theaters;

        public TheaterService()
        {
            _theaters = InitializeTheaters();
        }

        private List<MovieTheater> InitializeTheaters()
        {
            var theaters = new List<MovieTheater>
            {
                new MovieTheater(1, "City 1", "Downtown"),
                new MovieTheater(2, "City 2", "Mall Location"),
                new MovieTheater(3, "City 3", "Suburbs"),
                new MovieTheater(4, "City 4", "City Center")
            };

            // Add screens to each theater
            theaters[0].Screens.Add(new MovieScreen(1, "Screen 1"));
            theaters[0].Screens.Add(new MovieScreen(2, "Screen 2"));

            theaters[1].Screens.Add(new MovieScreen(1, "Screen 1"));
            theaters[1].Screens.Add(new MovieScreen(2, "Screen 2"));
            theaters[1].Screens.Add(new MovieScreen(3, "Screen 3"));

            theaters[2].Screens.Add(new MovieScreen(1, "Screen 1"));
            theaters[2].Screens.Add(new MovieScreen(2, "Screen 2"));

            theaters[3].Screens.Add(new MovieScreen(1, "Screen 1"));
            theaters[3].Screens.Add(new MovieScreen(2, "Screen 2"));
            theaters[3].Screens.Add(new MovieScreen(3, "Screen 3"));
            theaters[3].Screens.Add(new MovieScreen(4, "Screen 4"));

            return theaters;
        }

        public async Task<List<MovieTheater>> GetAllTheatersAsync()
        {
            // Simulate async operation
            await Task.Delay(50);
            return _theaters.ToList();
        }


        public async Task<MovieTheater> GetMovieTheaterByIdAsync(int id)
        {
            await Task.Delay(50);
            return _theaters.FirstOrDefault(t => t.Id == id);
        }

        public async Task<MovieTheater> GetMovieTheaterByNameAsync(string name)
        {
            await Task.Delay(50);
            return _theaters.FirstOrDefault(t => t.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<MovieScreen>> IMovieTheater.GetScreensForTheaterAsync(int theaterId)
        {
            await Task.Delay(50);
            var theater = _theaters.FirstOrDefault(t => t.Id == theaterId);
            return theater?.Screens.ToList() ?? new List<MovieScreen>();
        }

        public async Task<List<MovieScreen>> IMovieTheater.GetAvailableScreensForTheaterAsync(int theaterId)
        {
            await Task.Delay(50);
            var theater = _theaters.FirstOrDefault(t => t.Id == theaterId);
            return theater?.Screens.Where(s => s.IsAvailable).ToList() ?? new List<MovieScreen>();
        }
    }
}
