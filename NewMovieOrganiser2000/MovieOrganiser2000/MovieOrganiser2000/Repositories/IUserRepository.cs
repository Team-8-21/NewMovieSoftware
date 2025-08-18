using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSoftware.MVVM.Model.Classes;

namespace MovieSoftware.MVVM.Model.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetUser(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        bool ValidateUserLogin(string username, string password);

    }
}
