using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieOrganiser2000.ViewModels;
using MovieOrganiser2000.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MovieOrganiser2000.UnitTest
{
    [TestClass]
    public class AddMovieViewModel_NoRefactor_Tests
    {
        [TestMethod]
        //Naming convention: ClassName_MethodName_ExpectedBehavior

        public void Ctor_Initializes_Genres_And_Defaults()
        // Tester at konstruktøren sætter Genres til alle enum-værdier
        // og at alle properties starter med de rigtige default-værdier.
        {
            var vm = new AddMovieViewModel();

            // Genres indeholder alle enum-værdier
            var expected = ((Genre[])Enum.GetValues(typeof(Genre))).ToList();
            CollectionAssert.AreEquivalent(expected, vm.Genres.ToList());

            // Default valg
            Assert.AreEqual(Genre.Ukendt, vm.SelectedGenre);
            Assert.AreEqual("", vm.Title);
            Assert.AreEqual(0, vm.MovieLength);
            Assert.AreEqual("", vm.Director);
            Assert.AreEqual(default(DateOnly), vm.Premiere);
        }

        [TestMethod]
        public void PropertyChanged_Invoked_For_All_Properties()
        // Tester at PropertyChanged-eventet bliver rejst korrekt,
        // når man ændrer Title, MovieLength, Director, Premiere og SelectedGenre.
        {
            var vm = new AddMovieViewModel();
            var changed = new List<string>();
            vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            vm.Title = "Alien";
            vm.MovieLength = 117;
            vm.Director = "Ridley Scott";
            vm.Premiere = new DateOnly(1979, 5, 25);
            vm.SelectedGenre = Genre.SciFi;

            CollectionAssert.IsSubsetOf(
                new[] { "Title", "MovieLength", "Director", "Premiere", "SelectedGenre" },
                changed
            );
        }

        [TestMethod]
        public void AddMovieCommand_Resets_Fields()
        // Tester at AddMovieCommand kan køres, og at den efterfølgende
        // nulstiller alle inputfelter til deres default-værdier.
        {
            var vm = new AddMovieViewModel
            {
                Title = "Blade Runner",
                MovieLength = 114,
                Director = "Ridley Scott",
                SelectedGenre = Genre.SciFi,
                Premiere = new DateOnly(1982, 6, 25)
            };

            // RelayCommand har ingen CanExecute-predicate i din kode -> kan altid execute
            Assert.IsTrue(vm.AddMovieCommand.CanExecute(null));

            vm.AddMovieCommand.Execute(null);

            Assert.AreEqual("", vm.Title);
            Assert.AreEqual(0, vm.MovieLength);
            Assert.AreEqual("", vm.Director);
            Assert.AreEqual(Genre.Ukendt, vm.SelectedGenre);
            Assert.AreEqual(default(DateOnly), vm.Premiere);
        }
    }
}
