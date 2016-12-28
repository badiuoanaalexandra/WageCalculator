using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;

namespace WageCalculator.ViewModels
{
    /// <summary>
    /// Vizualize the wage calculations in a certain interval of time
    /// </summary>
    public class IntervalWage
    {
        public decimal TotalWage { get; set; }
        public decimal TotalEveningCompensation { get; set; }
        public decimal TotalOvertimeCompensation { get; set; }
        public decimal TotalWorktimeHours { get; set; }
        public decimal TotalEveningHours { get; set; }
        public decimal TotalOvertimeHours{ get; set; }
        public List<DailyWage> DailyWages { get; set; }
    }
}