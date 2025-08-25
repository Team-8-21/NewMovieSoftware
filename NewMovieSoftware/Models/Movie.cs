using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public int MovieLength { get; set; }
        public Genre Genre { get; set; }
        public string Director { get; set; }
        public DateOnly Premiere { get; set; }

        public Movie(string title, int movieLength, Genre genre, string director, DateOnly premiere) 
        {
            Title = title ?? "Ukendt";
            MovieLength = movieLength;
            Genre = genre;
            Director = director ?? "Ukendt";
            Premiere = premiere;
        }

    }
    public enum Genre
    {
        Ukendt = 0, //
        Action = 1,
        Krimi = 2,
        Historisk = 3,
        SciFi = 4,
        Krig = 5,
        Western = 6,
        Drama = 7,
        Romantik = 8,
        Fantasy = 9,
        Gyser = 10,
        Børn = 11

    }
}
