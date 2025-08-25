using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public Customer(string firstName, string lastName, string email, int phoneNumber)
        {
            FirstName = firstName ?? "Ukendt";
            LastName = lastName ?? "Ukendt";
            Email = email ?? "Ukendt";
            PhoneNumber = phoneNumber;

        }
    }
}
