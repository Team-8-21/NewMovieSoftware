using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSoftware.MVVM.Model.Classes
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{UserName}, {Password}";
        }

        public static User FromString(string input)
        {
            string[] parts = input.Split(',');
            return new User
            {
                UserName = parts[0],
                Password = parts[1]
            };
        }

    }
}
