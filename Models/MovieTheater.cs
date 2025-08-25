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
        public string TheaterAdress { get; set; } // Kunne i princippet også være en adresseliste eller Enum?
        public int TotalSeats { get; set; }

        public MovieTheater(int screen, string theaterAdress, int totalSeats)
        {
            Screen = screen;
            TheaterAdress = theaterAdress ?? "Ukendt";
            TotalSeats = totalSeats;

        }
    }
}

/* Forslag fra ChatGPT ift. nuværende struktur

public sealed class Biograf
{
    public Guid Id { get; init; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public List<Sal> Sale { get; } = new();
}

public sealed class Sal
{
    public Guid Id { get; init; }
    public string Nummer { get; set; } = "";
    public Guid BiografId { get; init; }
}

public readonly record struct Minutes(int Value)
{
    public static Minutes operator +(Minutes a, Minutes b) => new(a.Value + b.Value);
    public TimeSpan ToTimeSpan() => TimeSpan.FromMinutes(Value);
}

public sealed class Visning
{
    public Guid Id { get; init; }
    public DateOnly Date { get; private set; }
    public TimeOnly Start { get; private set; }
    public Guid FilmId { get; private set; }
    public Guid SalId { get; private set; }

    // Afledte beregninger – ingen storage
    public DateTime StartUtc(Func<DateTime> tzResolver) =>
        Date.ToDateTime(Start); // tilpas evt. timezone

    public Minutes TotalDuration(Minutes filmLength, Policy policy)
        => new(filmLength.Value + policy.Ads.Value + policy.Cleaning.Value);

    public DateTime EndTime(Film film, Policy policy)
        => Date.ToDateTime(Start).AddMinutes(TotalDuration(new Minutes(film.LengthMinutes), policy).Value);

    // Invariant: må ikke ligge før premiere
    public static Visning Create(DateOnly date, TimeOnly start, Film film, Sal sal)
    {
        if (date < film.Premiere)
            throw new ArgumentException("Visning før filmens premieredato er ikke tilladt.");
        return new Visning { Id = Guid.NewGuid(), Date = date, Start = start, FilmId = film.Id, SalId = sal.Id };
    }
}

public sealed class Policy
{
    public Minutes Ads { get; init; } = new(15);
    public Minutes Cleaning { get; init; } = new(15);
}


public sealed class ScheduleService
{
    private readonly Policy _policy;

    public ScheduleService(Policy policy) => _policy = policy;

    // Tjek at visninger i samme sal ikke overlapper
    public bool Overlaps(Visning a, Film fa, Visning b, Film fb)
    {
        var aStart = a.Date.ToDateTime(a.Start);
        var aEnd   = a.EndTime(fa, _policy);
        var bStart = b.Date.ToDateTime(b.Start);
        var bEnd   = b.EndTime(fb, _policy);
        return aStart < bEnd && bStart < aEnd;
    }
}
*/