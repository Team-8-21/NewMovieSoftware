using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieOrganiser2000.Helpers;
using MovieOrganiser2000.Models;
using MovieSoftware.MVVM.Model.Classes;
using Newtonsoft.Json;

namespace MovieOrganiser2000.Repositories
{
           public class FileMovieRepository : IMovieRepository
        {
        private readonly string _filePath;

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new DateOnlyJsonConverter() } // se min tidligere converter
        };

        public FileMovieRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath må ikke være tomt", nameof(filePath));

            _filePath = filePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            Debug.WriteLine($"[Repo] movies.json path: {_filePath}");
        }

        public IEnumerable<Movie> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Movie>();
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Movie>>(json, _jsonSettings) ?? new List<Movie>();
        }

        public Movie GetMovie(Movie movie) => GetAll().FirstOrDefault(m => m.Title == movie.Title);

        public void AddMovie(Movie movie)
        {
            var movies = GetAll().ToList();
            movies.Add(movie);
            SaveAll(movies);
        }

        public void UpdateMovie(Movie movie)
        {
            var movies = GetAll().ToList();
            var idx = movies.FindIndex(m => m.Title == movie.Title);
            if (idx == -1) throw new KeyNotFoundException("Film ikke fundet");
            movies[idx] = movie;
            SaveAll(movies);
        }

        public void DeleteMovie(Movie movie)
        {
            var movies = GetAll().ToList();
            movies.RemoveAll(m => m.Title == movie.Title);
            SaveAll(movies);
        }

        private void SaveAll(IEnumerable<Movie> movies)
        {
            var json = JsonConvert.SerializeObject(movies, _jsonSettings);
            File.WriteAllText(_filePath, json);
        }
    }
}
