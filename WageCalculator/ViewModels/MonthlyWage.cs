using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;

namespace WageCalculator.ViewModels
{
    public class MonthlyWage
    {
        public decimal TotalMonthlyWage { get; set; }
        public decimal TotalEveningCompensation { get; set; }
        public decimal TotalOvertimeCompensation { get; set; }
        public decimal TotalWorktimeHours { get; set; }
        public decimal TotalEveningHours { get; set; }
        public decimal TotalOvertimeHours{ get; set; }

        public List<DailyWage> DailyWages { get; set; }

        public MonthlyWage(List<WorkingDay> workingDays, WagePricing wagePricing)
        {
            DailyWages = new List<DailyWage>();
            foreach (var workingDay in workingDays)
            {
                var dailyWage = new DailyWage(workingDay, wagePricing);
                TotalMonthlyWage += dailyWage.TotalWage;
                TotalEveningCompensation += dailyWage.EveningCompensation;
                TotalOvertimeCompensation += dailyWage.OvertimeWage;
                TotalEveningHours += dailyWage.EveningHours;
                TotalWorktimeHours += dailyWage.WorkingHours;
                TotalOvertimeHours += dailyWage.OvertimeHours;
                DailyWages.Add(dailyWage); 
            }
        }
    }
}