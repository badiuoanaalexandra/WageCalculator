using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.ViewModels;

namespace WageCalculator.Models
{
    /// <summary>
    /// DailyWageModel contains logic for calculating daily wage
    /// </summary>
    public class DailyWageModel
    {
        //Helper class
        private class Wage
        {
            public decimal Amount { get; set; }
            public decimal Hours { get; set; }
        }

        /// <summary>
        /// Wage compensationPlan based on which the calculation are done
        /// </summary>
        public WagePricing WagePricing { get; }
        /// <summary>
        /// Working day based on which the calculation are done
        /// </summary>
        public WorkingDay WorkingDay { get; }

        public DailyWageModel(WorkingDay workingDay, WagePricing wagePricing)
        {
            WagePricing = wagePricing;
            WorkingDay = workingDay;
        }

        /// <summary>
        /// Calculates daily wage for a working day
        /// </summary>
        /// <returns>DailyWage view model</returns>
        public DailyWage CalculateDailyWage()
        {
            var normalWage = CalculateNormalWage();
            var eveningCompensation = CalculateEveningCompensation();
            var overtimeCompensation = CalculateOvertimeCompensation(normalWage);

           return new DailyWage
           {
               WagePricing = WagePricing,
               WorkingDay = WorkingDay,
               EveningCompensation = Math.Round(eveningCompensation.Amount,2,MidpointRounding.AwayFromZero),
               EveningHours = Math.Round(eveningCompensation.Hours, 2, MidpointRounding.AwayFromZero),
               NormalHoursWage = Math.Round(normalWage.Amount, 2, MidpointRounding.AwayFromZero),
               WorkingHours = Math.Round(normalWage.Hours, 2, MidpointRounding.AwayFromZero),
               OvertimeCompensation = Math.Round(overtimeCompensation.Amount, 2, MidpointRounding.AwayFromZero),
               OvertimeHours = Math.Round(overtimeCompensation.Hours,2, MidpointRounding.AwayFromZero)
           };
        }

        /// <summary>
        /// Calculates normal wage 
        /// </summary>
        /// <returns>Wage object</returns>
        private Wage CalculateNormalWage()
        {
            var wage = new Wage();
            foreach (var dailyHour in WorkingDay.WorkingShifts)
            {
                wage.Hours += Convert.ToDecimal((dailyHour.EndTime - dailyHour.StartTime).TotalHours);
            }

            wage.Amount = wage.Hours*WagePricing.BasicHourlyWage;
            return wage;
        }

        /// <summary>
        /// Calculates overtime compensation (the normal wage for these hours not included in the compensation)
        /// </summary>
        /// <param name="normalWage">Wage object calculated with CalculateNormalWage() method</param>
        /// <returns>Wage object</returns>
        private Wage CalculateOvertimeCompensation(Wage normalWage)
        {
            var overtimeCompensation = new Wage();
            overtimeCompensation.Hours = normalWage.Hours - WagePricing.BasicDayHours;
            if (overtimeCompensation.Hours <= 0)
            {
                overtimeCompensation.Hours = 0;
                return overtimeCompensation;
            }

            var overtimeHours = overtimeCompensation.Hours;
            foreach (var overtimePricing in WagePricing.OvertimeCompensationPlans.OrderBy(o => o.ApplyOrder))
            {
                overtimeCompensation.Amount += CalculateCompensationPlanOvertime(overtimePricing, ref overtimeHours);
                if (overtimeHours < 0)
                {
                    return overtimeCompensation;
                }
            }

            return overtimeCompensation;
        }

        /// <summary>
        /// Calculates compensation for one overtime plan (for example first 2 hours)
        /// </summary>
        /// <param name="compensationPlan">OvertimeCompensationPlan plan</param>
        /// <param name="workingHours">workingHours</param>
        /// <returns>Overtime compensation based on overtime compensationPlan plan (decimal)</returns>
        private decimal CalculateCompensationPlanOvertime(OvertimeCompensationPlan compensationPlan, ref decimal workingHours)
        {
            var originalWorkingHours = workingHours;
            workingHours = workingHours - compensationPlan.HourTimeSpan;
            if (workingHours > 0)
            {
                return WagePricing.BasicHourlyWage * compensationPlan.Percentage * compensationPlan.HourTimeSpan;
            }

            return WagePricing.BasicHourlyWage * compensationPlan.Percentage * originalWorkingHours;
        }

        /// <summary>
        /// Calculates evening compensation (normal wage for the same hours not included in the compensation)
        /// </summary>
        /// <returns>Wage object</returns>
        private Wage CalculateEveningCompensation()
        {
            var eveningWage = new Wage();
            foreach (var workingShift in WorkingDay.WorkingShifts)
            {
                var shiftEveningWage = CalculateWorkingShiftEveningCompensation(workingShift);
                eveningWage.Amount += shiftEveningWage.Amount;
                eveningWage.Hours += shiftEveningWage.Hours;
            }

            return eveningWage;
        }

        /// <summary>
        /// Calcualates evening compensation for one working shift
        /// </summary>
        /// <param name="workingShift">WorkingShift object</param>
        /// <returns>Wage object</returns>
        private Wage CalculateWorkingShiftEveningCompensation(WorkingShift workingShift)
        {
            var shiftEveningWage = new Wage();
            var eveningHours = 0M;

            eveningHours += CalculateEveningHours_ShiftStartingAfterMidnight(workingShift);

            eveningHours += CalculateEveningHours_ShiftLastedOverEveningEndHours(workingShift);

            eveningHours += CalculateEveningHours_ShiftEndsAfterMidnight(workingShift);

            eveningHours += CalculateEveningHours_ShiftStartsBeforeEveningEndsAfterEvening(workingShift);

            if (eveningHours < 0)
            {
                throw new Exception("Evening hours should never be negative!");
            }

            shiftEveningWage.Hours = Convert.ToDecimal(eveningHours);
            shiftEveningWage.Amount = Convert.ToDecimal(eveningHours)*WagePricing.EveningPricing.Compensation;
            return shiftEveningWage;
        }

        /// <summary>
        /// Calculates evening hours for shifts that start after midnight. The shifts can end in the morning after the evening hours passed
        /// </summary>
        /// <param name="workingShift">Object WorkingShift</param>
        /// <returns>Evening Hours (Decimal)</returns>
        private decimal CalculateEveningHours_ShiftStartingAfterMidnight(WorkingShift workingShift)
        {
            var eveningHours = 0M;
            if (workingShift.StartTime.Hour <= WagePricing.EveningPricing.EndHour)
            {
                if (workingShift.EndTime.Hour >= WagePricing.EveningPricing.EndHour)
                {
                    eveningHours += WagePricing.EveningPricing.EndHour - (workingShift.StartTime.Hour +
                                    (Convert.ToDecimal(workingShift.StartTime.Minute) / 60M));
                }
                else
                {
                    eveningHours += (workingShift.EndTime.Hour + Convert.ToDecimal(workingShift.EndTime.Minute) / 60M) - (workingShift.StartTime.Hour +
                                   (Convert.ToDecimal(workingShift.StartTime.Minute) / 60M));
                }
            }

            return eveningHours;
        }

        /// <summary>
        /// Calculates evening hours for shifts that started before midnight and ended later in the morning when evening hours passed.
        /// </summary>
        /// <param name="workingShift">Object WorkingShift</param>
        /// <returns>Evening Hours (Decimal)</returns>
        private decimal CalculateEveningHours_ShiftLastedOverEveningEndHours(WorkingShift workingShift)
        {
            var eveningHours = 0M;
            if (workingShift.EndTime.Hour >= WagePricing.EveningPricing.StartHour)
            {
                if (workingShift.StartTime.Hour <= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += (workingShift.EndTime.Hour + Convert.ToDecimal(workingShift.EndTime.Minute) / 60M) -
                                    WagePricing.EveningPricing.StartHour;
                }
                else
                {
                    eveningHours += (workingShift.EndTime.Hour + Convert.ToDecimal(workingShift.EndTime.Minute) / 60M) -
                                    (workingShift.StartTime.Hour + Convert.ToDecimal(workingShift.StartTime.Minute) / 60M);
                }
            }

            return eveningHours;
        }

        /// <summary>
        /// Calculates evening hours for shifts that started before midnight and end after midnight, but before evening hours has passed
        /// </summary>
        /// <param name="workingShift">Object WorkingShift</param>
        /// <returns>Evening Hours (Decimal)</returns>
        private decimal CalculateEveningHours_ShiftEndsAfterMidnight(WorkingShift workingShift)
        {
            var eveningHours = 0M;
            if (workingShift.EndTime.Hour <= WagePricing.EveningPricing.EndHour && workingShift.StartTime.Hour > workingShift.EndTime.Hour)
            {
                if (workingShift.StartTime.Hour >= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += 24 - workingShift.StartTime.Hour - (workingShift.StartTime.Minute / 60) +
                                    workingShift.EndTime.Hour + (workingShift.EndTime.Minute / 60);
                }
                else
                {
                    eveningHours += 24 - WagePricing.EveningPricing.StartHour - (workingShift.StartTime.Minute / 60) +
                                    workingShift.EndTime.Hour + (workingShift.EndTime.Minute / 60);
                }
            }

            return eveningHours;
        }
        /// <summary>
        /// Calculates evening hours for shifts that started before evening hours and ended after evening hours has passed (very long shift)
        /// </summary>
        /// <param name="workingShift">Object WorkingShift</param>
        /// <returns>Evening Hours (Decimal)</returns>
        private decimal CalculateEveningHours_ShiftStartsBeforeEveningEndsAfterEvening(WorkingShift workingShift)
        {
            var eveningHours = 0M;
            if (workingShift.EndTime.Hour > WagePricing.EveningPricing.EndHour && workingShift.StartTime.Hour > workingShift.EndTime.Hour)
            {
                if (workingShift.StartTime.Hour >= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += 24 - workingShift.StartTime.Hour - (workingShift.StartTime.Minute / 60) +
                                    WagePricing.EveningPricing.EndHour;
                }
                else
                {
                    eveningHours += 24 - WagePricing.EveningPricing.StartHour - (workingShift.StartTime.Minute / 60) +
                                    WagePricing.EveningPricing.EndHour;
                }
            }

            return eveningHours;
        }

    }
}