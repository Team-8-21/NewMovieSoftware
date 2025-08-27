using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Theater { get; set; }
        public string Screen { get; set; }
    }
}
