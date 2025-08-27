using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieOrganiser2000.Models;

    namespace ScheduleOrganiser2000.Repositories
    {
        public interface IScheduleRepository
        {
            IEnumerable<Schedule> GetAll();
            Schedule GetSchedule(Schedule schedule);
            void AddSchedule(Schedule schedule);
            void UpdateSchedule(Schedule schedule);
            void DeleteSchedule(Schedule schedule);
        }
    }

