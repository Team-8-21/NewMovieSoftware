using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieOrganiser2000.Models;
using MovieOrganiser2000.ViewModels;
using MovieOrganiser2000.Repositories;

using MovieSoftware.MVVM.Model.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MovieSoftware.MVVM.Model.Classes;
using System.Windows.Documents;

namespace MovieOrganiser2000.UnitTest
{
    [TestClass]
    public class AddMovieViewModel_NoRefactor_Tests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        //Naming convention: ClassName_MethodName_ExpectedBehavior

        public void Ctor_Initializes_Genres_And_Defaults()
        // Tester at konstruktøren sætter Genres til alle enum-værdier
        // og at alle properties starter med de rigtige default-værdier.
        {
            var vm = new AddMovieViewModel();

            TestContext.WriteLine($"Init values -> Title:'{vm.Title}', Director:'{vm.Director}', Genre:{vm.SelectedGenre}, Length:{vm.MovieLength}, Premiere:{vm.Premiere}");

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

            // Log alle ændringer
            TestContext.WriteLine("Følgende properties fired PropertyChanged:");
            foreach (var c in changed)
                TestContext.WriteLine($" - {c}");


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

            // Log efter reset
            TestContext.WriteLine($"Efter AddMovieCommand.Execute:");
            TestContext.WriteLine($" Title = '{vm.Title}'");
            TestContext.WriteLine($" MovieLength = {vm.MovieLength}");
            TestContext.WriteLine($" Director = '{vm.Director}'");
            TestContext.WriteLine($" SelectedGenre = {vm.SelectedGenre}");
            TestContext.WriteLine($" Premiere = {vm.Premiere}");

            Assert.AreEqual("", vm.Title);
            Assert.AreEqual(0, vm.MovieLength);
            Assert.AreEqual("", vm.Director);
            Assert.AreEqual(Genre.Ukendt, vm.SelectedGenre);
            Assert.AreEqual(default(DateOnly), vm.Premiere);
        }
    }

        [TestClass]
        public class FileUserRepository_Tests
        {
            public TestContext TestContext { get; set; }
            private string _tempFile;

            [TestInitialize]
            public void Setup()
            {
                // Opretter en tom temp-fil før hver test, så repo’et arbejder mod en ren fil (ingen seed-data).
                _tempFile = Path.GetTempFileName();
                File.WriteAllText(_tempFile, string.Empty);
                TestContext.WriteLine("Tempfil oprettet: " + _tempFile);
            }

            [TestCleanup]
            public void Cleanup()
            {
                // Sletter temp-filen efter hver test, så tests er isolerede og uden sideeffekter.
                try
                {
                    if (File.Exists(_tempFile))
                    {
                        File.Delete(_tempFile);
                        TestContext.WriteLine("Tempfil slettet: " + _tempFile);
                    }
                }
                catch
                {
                    // Best effort
                }
            }

            [TestMethod]
            public void Ctor_CreatesFile_And_Seeds_When_File_DoesNotExist()
            {
                // Tester at konstruktøren opretter filen hvis den mangler,
                // og at standardbrugeren "Jens.Hansen" seedes i en ny fil.

                // Arrange: slet tempfilen, så ctor tror den ikke findes
                File.Delete(_tempFile);
                Assert.IsFalse(File.Exists(_tempFile), "Forudsætning: filen må ikke findes før ctor.");

                // Act
                var repo = new FileUserRepository(_tempFile);

                // Assert
                Assert.IsTrue(File.Exists(_tempFile), "Ctor skulle oprette filen.");

                List<User> users = new List<User>(repo.GetAll());
                bool foundSeed = false;
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i] != null && users[i].UserName == "Jens.Hansen")
                    {
                        foundSeed = true;
                        break;
                    }
                }

                // Log indsigt
                TestContext.WriteLine("Brugere efter ctor:");
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i] != null)
                        TestContext.WriteLine(" - " + users[i].UserName + ", " + users[i].Password);
                }

                Assert.IsTrue(foundSeed, "Ctor skulle seed’e standardbrugeren når filen ikke findes.");
            }

            [TestMethod]
            public void AddUser_Appends_And_GetAll_Parses()
            {
                // Tester at AddUser tilføjer linjer til filen, og at GetAll kan læse dem korrekt tilbage.

                var repo = new FileUserRepository(_tempFile);
                var u1 = new User { UserName = "alice", Password = "pw" };
                var u2 = new User { UserName = "bob", Password = "secret" };

                // Act
                repo.AddUser(u1);
                repo.AddUser(u2);

                // Assert
                List<User> all = new List<User>(repo.GetAll());

                // Log indhold
                TestContext.WriteLine("Alle brugere efter AddUser:");
                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i] != null)
                        TestContext.WriteLine(" - " + all[i].UserName + ", " + all[i].Password);
                }

                Assert.AreEqual(2, all.Count, "Der burde være præcis 2 brugere.");

                bool hasAlice = false;
                bool hasBob = false;

                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i] != null && all[i].UserName == "alice" && all[i].Password == "pw")
                        hasAlice = true;
                    if (all[i] != null && all[i].UserName == "bob" && all[i].Password == "secret")
                        hasBob = true;
                }

                Assert.IsTrue(hasAlice, "Kunne ikke finde 'alice' med korrekt password.");
                Assert.IsTrue(hasBob, "Kunne ikke finde 'bob' med korrekt password.");
            }

            [TestMethod]
            public void GetUser_Returns_Correct_User_Or_Null()
            {
                // Tester at GetUser finder den rigtige bruger ved username, og returnerer null hvis brugeren ikke findes.

                var repo = new FileUserRepository(_tempFile);
                repo.AddUser(new User { UserName = "alice", Password = "pw" });

                // Act
                User found = repo.GetUser("alice");
                User missing = repo.GetUser("charlie");

                // Log
                TestContext.WriteLine("GetUser('alice') -> " + (found == null ? "null" : found.UserName + ", " + found.Password));
                TestContext.WriteLine("GetUser('charlie') -> " + (missing == null ? "null" : missing.UserName));

                // Assert
                Assert.IsNotNull(found, "Skulle returnere en bruger for 'alice'.");
                Assert.AreEqual("alice", found.UserName, "Forkert brugernavn returneret.");
                Assert.IsNull(missing, "Skulle returnere null for en manglende bruger.");
            }

            [TestMethod]
            public void UpdateUser_Rewrites_Line()
            {
                // Tester at UpdateUser opdaterer den eksisterende bruger (samme username) og omskriver filen korrekt.

                var repo = new FileUserRepository(_tempFile);
                repo.AddUser(new User { UserName = "alice", Password = "pw" });

                // Act: opdater password
                repo.UpdateUser(new User { UserName = "alice", Password = "newpw" });

                // Assert
                List<User> all = new List<User>(repo.GetAll());

                // Log
                TestContext.WriteLine("Efter UpdateUser:");
                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i] != null)
                        TestContext.WriteLine(" - " + all[i].UserName + ", " + all[i].Password);
                }

                Assert.AreEqual(1, all.Count, "Der burde stadig kun være 1 bruger.");
                Assert.AreEqual("newpw", all[0].Password, "Password blev ikke opdateret korrekt.");
            }

            [TestMethod]
            public void DeleteUser_Removes_Line()
            {
                // Tester at DeleteUser fjerner den angivne bruger fra filen, så kun de resterende brugere er tilbage.

                var repo = new FileUserRepository(_tempFile);
                repo.AddUser(new User { UserName = "alice", Password = "pw" });
                repo.AddUser(new User { UserName = "bob", Password = "secret" });

                // Act
                repo.DeleteUser(new User { UserName = "alice" });

                // Assert
                List<User> all = new List<User>(repo.GetAll());

                // Log
                TestContext.WriteLine("Efter DeleteUser('alice'):");
                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i] != null)
                        TestContext.WriteLine(" - " + all[i].UserName + ", " + all[i].Password);
                }

                Assert.AreEqual(1, all.Count, "Der burde kun være 1 bruger tilbage.");
                Assert.AreEqual("bob", all[0].UserName, "Forkert bruger blev tilbage efter sletning.");
            }

            [TestMethod]
            public void ValidateUserLogin_Returns_True_When_Match_Else_False()
            {
                // Tester at ValidateUserLogin returnerer true ved korrekt username+password,
                // og false ved forkert password eller manglende bruger.

                var repo = new FileUserRepository(_tempFile);
                repo.AddUser(new User { UserName = "alice", Password = "pw" });

                // Act
                bool ok = repo.ValidateUserLogin("alice", "pw");
                bool wrongPw = repo.ValidateUserLogin("alice", "wrong");
                bool missing = repo.ValidateUserLogin("missing", "pw");

                // Log
                TestContext.WriteLine("Login(alice, pw) -> " + ok);
                TestContext.WriteLine("Login(alice, wrong) -> " + wrongPw);
                TestContext.WriteLine("Login(missing, pw) -> " + missing);

                // Assert
                Assert.IsTrue(ok, "Korrekte credentials burde give true.");
                Assert.IsFalse(wrongPw, "Forkert password burde give false.");
                Assert.IsFalse(missing, "Manglende bruger burde give false.");
            }
        }
    }

