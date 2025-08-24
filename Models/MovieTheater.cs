using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class MovieTheater
    {
        public int Screen { get; set; }
        public string TheaterAdress { get; set; } // Kunne i princippet også være en adresse eller Enum?
        public int TotalSeats { get; set; }

        public MovieTheater(int screen, string theaterAdress, int totalSeats)
        {
            Screen = screen;
            TheaterAdress = theaterAdress ?? "Ukendt";
            TotalSeats = totalSeats;

        }
    }
}
