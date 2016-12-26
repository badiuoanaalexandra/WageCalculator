using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    /// <summary>
    /// Working day includes all the shifts during that day. Shifts that go past midnight are also included in the same working day
    /// </summary>
    public class WorkingDay
    {
        public WorkingDay()
        {
            WorkingShifts = new List<WorkingShift>();
        }

        public long WorkingDayId { get; set; }
        public DateTime Date { get; set; }

        public List<WorkingShift> WorkingShifts { get; set; }
    }
}