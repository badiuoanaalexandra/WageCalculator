using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    /// <summary>
    /// Working shift during a working day
    /// </summary>
    public class WorkingShift
    {
        public long WorkingShiftId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}