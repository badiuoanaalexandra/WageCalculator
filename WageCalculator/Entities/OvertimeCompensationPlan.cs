using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    /// <summary>
    /// Overtime compensation plan
    /// </summary>
    public class OvertimeCompensationPlan
    {
        public int ApplyOrder { get; set; }
        public decimal Percentage { get; set; }
        public int HourTimeSpan { get; set; }
    }
}