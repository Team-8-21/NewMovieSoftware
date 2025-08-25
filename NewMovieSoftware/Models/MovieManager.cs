using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class MovieManager
    {
        private ObservableCollection<Movie> _DatabaseMovies = new ObservableCollection<Movie>()
        { };

        public ObservableCollection<Movie> GetMovies()
        {
            return _DatabaseMovies;
        }

        public void AddMovie(Movie movie)
        {
            _DatabaseMovies.Add(movie);
        }
    }
}
