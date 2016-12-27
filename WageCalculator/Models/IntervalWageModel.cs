using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.ViewModels;

namespace WageCalculator.Models
{
    /// <summary>
    /// IntervalWageModel calculates the wage for a given list of working days
    /// </summary>
    public class IntervalWageModel
    {
        /// <summary>
        /// Working days belonging to an interval
        /// </summary>
        public List<WorkingDay> WorkingDays { get; }
        /// <summary>
        /// Wage pricing based on which the calculation are done
        /// </summary>
        public WagePricing WagePricing { get; }

        public IntervalWageModel(List<WorkingDay> workingDays, WagePricing wagePricing)
        {
            WorkingDays = workingDays;
            WagePricing = wagePricing;
        }

        /// <summary>
        /// Calculates wage for an interval of time
        /// </summary>
        /// <returns>IntervalWage object</returns>
        public IntervalWage CalculateIntervalWage()
        {
            var intervalWage = new IntervalWage();
            foreach (var workingDay in WorkingDays)
            {
                var dailyWage = new DailyWageModel(workingDay, WagePricing).CalculateDailyWage();
                intervalWage.TotalWage += dailyWage.TotalWage;
                intervalWage.TotalEveningCompensation += dailyWage.EveningCompensation;
                intervalWage.TotalOvertimeCompensation += dailyWage.OvertimeCompensation;
                intervalWage.TotalEveningHours += dailyWage.EveningHours;
                intervalWage.TotalWorktimeHours += dailyWage.WorkingHours;
                intervalWage.TotalOvertimeHours += dailyWage.OvertimeHours;
                intervalWage.DailyWages.Add(dailyWage);
            }

            return intervalWage;
        }
    }
}