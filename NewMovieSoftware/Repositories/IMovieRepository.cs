using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieOrganiser2000.Models;
using MovieSoftware.MVVM.Model.Classes;
using Newtonsoft.Json;

namespace MovieOrganiser2000.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie GetMovie(Movie movie);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(Movie movie);        
    }
}
