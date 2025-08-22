using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class Show
    {
        public DateOnly DateOnly { get; set; }
        public TimeOnly Time { get; set; }
        public int TotalTime { get; set; } //Total minutter - eller hvad gør vi her?

    }
}
