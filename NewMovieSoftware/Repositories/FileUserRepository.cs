using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSoftware.MVVM.Model.Classes;

namespace MovieSoftware.MVVM.Model.Repositories
{
    public class FileUserRepository : IUserRepository
    {
        private readonly string _userFilePath;

        public FileUserRepository(string userFilePath)
        {
            _userFilePath = userFilePath;
            if (!File.Exists(_userFilePath))
            {
                File.Create(_userFilePath).Close();
                User client = new User { UserName = "Jens.Hansen", Password = "TheMovies123" };
                AddUser(client);
            }
        }

        public void AddUser(User user)
        {
            try
            {
                File.AppendAllText(_userFilePath, user.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved omskrivning til fil: {ex.Message}");
            }
        }

        public void DeleteUser(User user)
        {
            List<User> users = GetAll().ToList();
            users.RemoveAll(u => u.UserName == user.UserName);
            RewriteFile(users);
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return File.ReadAllLines(_userFilePath)
                           .Where(line => !string.IsNullOrEmpty(line))
                           .Select(User.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public User GetUser(string username)
        {
            return GetAll().FirstOrDefault(u => u.UserName == username);
        }

        public void UpdateUser(User user)
        {
            List<User> users = GetAll().ToList();
            int index = users.FindIndex(u => u.UserName == user.UserName);
            if(index != -1)
            {
                users[index] = user;
                RewriteFile(users);
            }
        }

        public bool ValidateUserLogin(string username, string password)
        {
            try
            {
                foreach (User user in GetAll())
                {
                    var storedUserName = user.UserName;
                    var storedPassword = user.Password;
                    if (storedUserName == username && storedPassword == password)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved indlæsning af fil: {ex.Message}");
                return false;
            }
        }

        private void RewriteFile(List<User> users)
        {
            try
            {
                File.WriteAllLines(_userFilePath, users.Select(u => u.ToString()));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved indlæsning til fil: {ex.Message}");
            }
        }
    }
}
