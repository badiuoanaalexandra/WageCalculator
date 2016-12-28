using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
    /// <summary>
    /// Visualize the daily wage calculations
    /// </summary>
    public class DailyWage
    {
        public decimal TotalWage => NormalHoursWage + EveningCompensation + OvertimeCompensation;
        public decimal NormalHoursWage { get; set; }
        public decimal EveningCompensation { get; set; }
        public decimal OvertimeCompensation { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal EveningHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public WorkingDay WorkingDay { get; set; }
        public WagePricing WagePricing { get; set; }
    }
}