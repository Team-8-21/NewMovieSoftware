using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieOrganiser2000.Models;

namespace MovieOrganiser2000.Repositories
{
    public interface IMovieTheater
    {
        Task<List<MovieTheater>> GetAllTheatersAsync();
        Task<MovieTheater> GetMovieTheaterByIdAsync(int id);
        Task<MovieTheater> GetMovieTheaterByNameAsync(string name);
        Task<List<MovieScreen>> GetScreensForTheaterAsync(int theaterId);
        Task<List<MovieScreen>> GetAvailableScreensForTheaterAsync(int theaterId);
    }
}
