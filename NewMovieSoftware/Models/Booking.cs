using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class Booking
    {
       public int Amount { get; set; }
        public Customer Customer { get; set; }

        public Booking(int amount, Customer customer)
        {
            Amount = amount;
            Customer = customer;
        }


    }
}
