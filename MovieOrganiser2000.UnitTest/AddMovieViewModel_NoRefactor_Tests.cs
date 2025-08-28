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
        private string _tempFile;

        [TestInitialize]
        public void Setup()
        {
            // Kører før hver test
            // Opretter en midlertidig fil og skriver en tom JSON-liste ind
            _tempFile = Path.GetTempFileName();
            File.WriteAllText(_tempFile, "[]");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Kører efter hver test
            // Sletter temp-filen så vi ikke efterlader rod
            try { if (File.Exists(_tempFile)) File.Delete(_tempFile); } catch { }
        }

        [TestMethod]
        public void Ctor_Initializes_Genres_And_Defaults()
        {
            var repo = new FileMovieRepository(_tempFile);
            var vm = new AddMovieViewModel(repo);

            // Tjekker at alle enum-værdier fra Genre findes i VM
            var expected = ((Genre[])Enum.GetValues(typeof(Genre))).ToList();
            CollectionAssert.AreEquivalent(expected, vm.Genres.ToList());

            // Tjekker at alle properties starter med korrekte default-værdier
            Assert.AreEqual(Genre.Ukendt, vm.SelectedGenre);
            Assert.AreEqual("", vm.Title);
            Assert.AreEqual(0, vm.MovieLength);
            Assert.AreEqual("", vm.Director);
            Assert.AreEqual(default(DateOnly), vm.Premiere);

            TestContext.WriteLine("Ctor_Initializes_Genres_And_Defaults OK");
        }

        [TestMethod]
        public void PropertyChanged_Invoked_For_All_Properties()
        {
            var repo = new FileMovieRepository(_tempFile);
            var vm = new AddMovieViewModel(repo);

            // Samler hvilke properties der ændres
            var changed = new List<string>();
            vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            // Ændrer alle relevante properties
            vm.Title = "Alien";
            vm.MovieLength = 117;
            vm.Director = "Ridley Scott";
            vm.Premiere = new DateOnly(1979, 5, 25);
            vm.SelectedGenre = Genre.SciFi;

            // Tjekker at alle de properties har fyret PropertyChanged event
            CollectionAssert.IsSubsetOf(
                new[] { "Title", "MovieLength", "Director", "Premiere", "SelectedGenre" },
                changed
            );

            TestContext.WriteLine("PropertyChanged events fired for: " + string.Join(", ", changed));
        }

        [TestMethod]
        public void AddMovieCommand_Resets_Fields()
        {
            var repo = new FileMovieRepository(_tempFile);
            var vm = new AddMovieViewModel(repo)
            {
                // Fylder VM med testdata
                Title = "Blade Runner",
                MovieLength = 114,
                Director = "Ridley Scott",
                SelectedGenre = Genre.SciFi,
                Premiere = new DateOnly(1982, 6, 25)
            };

            // Sikrer at kommandoen kan køre
            Assert.IsTrue(vm.AddMovieCommand.CanExecute(null));

            // Kører kommandoen (skal tilføje film og nulstille felterne)
            vm.AddMovieCommand.Execute(null);

            // Tjekker at felterne blev nulstillet
            Assert.AreEqual("", vm.Title);
            Assert.AreEqual(0, vm.MovieLength);
            Assert.AreEqual("", vm.Director);
            Assert.AreEqual(Genre.Ukendt, vm.SelectedGenre);
            Assert.AreEqual(default(DateOnly), vm.Premiere);

            // Læser fra repo igen for at sikre at filmen faktisk blev gemt i JSON
            var after = new FileMovieRepository(_tempFile).GetAll().ToList();
            Assert.AreEqual(1, after.Count);
            Assert.AreEqual("Blade Runner", after[0].Title);

            TestContext.WriteLine("Efter AddMovieCommand: Gemte film = " + after[0].Title);
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
                { }
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

            TestContext.WriteLine("Brugere efter ctor: " + string.Join(", ", users.Select(u => u?.UserName)));

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
    [TestClass]
    public class Booking_Tests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Constructor_Sets_Amount_And_Customer()
        {
            // Arrange: lav en kunde som matcher Customer-ctor
            var customer = new Customer("Alice", "Andersen", "alice@example.com", 12345678);

            // Act
            var booking = new Booking(3, customer);

            // Assert: kræver at Booking-ctor sætter Amount = amount
            Assert.AreEqual(3, booking.Amount, "Konstruktøren burde sætte Amount fra parameteren.");
            Assert.AreSame(customer, booking.Customer, "Konstruktøren burde sætte Customer-referencen korrekt.");
          
            TestContext.WriteLine($"Booking: Amount={booking.Amount}, Customer={booking.Customer.FirstName}");

        }

        [TestMethod]
        public void Properties_Can_Be_Changed_After_Creation()
        {
            var customer = new Customer("Bob", "Bentsen", "bob@example.com", 87654321);
            var booking = new Booking(1, customer);

            // Act: ændr properties efter oprettelse
            booking.Amount = 5;
            booking.Customer = new Customer("Carla", "Christensen", "carla@example.com", 11112222);

            // Assert
            Assert.AreEqual(5, booking.Amount, "Amount skal kunne opdateres.");
            Assert.AreEqual("Carla", booking.Customer.FirstName, "Customer skal kunne udskiftes.");
           
            TestContext.WriteLine($"Changed booking: Amount={booking.Amount}, NewCustomer={booking.Customer.FirstName}");

        }

        [TestMethod]
        public void Customer_Ctor_Sets_Defaults_When_Nulls_Are_Passed()
        {
            // Arrange/Act: Customer håndterer nulls med "Ukendt"
            var c = new Customer(null, null, null, 0);

            // Assert
            Assert.AreEqual("Ukendt", c.FirstName);
            Assert.AreEqual("Ukendt", c.LastName);
            Assert.AreEqual("Ukendt", c.Email);
            Assert.AreEqual(0, c.PhoneNumber);

            TestContext.WriteLine("Customer med nulls -> defaults sat til 'Ukendt'");

        }
    }
}

