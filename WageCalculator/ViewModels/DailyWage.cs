using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
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

        public DailyWage(WorkingDay workingDay, WagePricing wagePricing)
        {
            WorkingDay = workingDay;
            WagePricing = wagePricing;

            CalculateNormalWage();
            NormalHoursWage = Math.Round(NormalHoursWage, 2, MidpointRounding.AwayFromZero);

            CalculateEveningCompensation();
            EveningCompensation = Math.Round(EveningCompensation, 2, MidpointRounding.AwayFromZero);

            CalculateOvertime();
            OvertimeCompensation = Math.Round(OvertimeCompensation, 2, MidpointRounding.AwayFromZero);
        }

        private void CalculateNormalWage()
        {
            foreach (var dailyHour in WorkingDay.WorkingShifts)
            {
                WorkingHours += Convert.ToDecimal((dailyHour.EndTime - dailyHour.StartTime).TotalHours);
            }

            NormalHoursWage = WorkingHours*WagePricing.BasicHourlyWage;
        }

        private void CalculateOvertime()
        {
            OvertimeHours = WorkingHours - WagePricing.BasicDayHours;
            if (OvertimeHours <= 0)
            {
                return;
            }

            var overtimeHours = OvertimeHours;
            foreach (var overtimePricing in WagePricing.OvertimePricings.OrderBy(o=> o.ApplyOrder))
            {
                OvertimeCompensation += CalculatePricingOvertime(overtimePricing, ref overtimeHours);
                if (overtimeHours < 0)
                {
                    return;
                }
            }
        }

        private decimal CalculatePricingOvertime(OvertimePricing pricing, ref decimal workingHours)
        {
            var originalWorkingHours = workingHours;
            workingHours = workingHours - pricing.HourTimeSpan;
            if (workingHours > 0)
            {
                return WagePricing.BasicHourlyWage*pricing.Percentage*pricing.HourTimeSpan;
            }

            return WagePricing.BasicHourlyWage * pricing.Percentage * originalWorkingHours;
        }

        private void CalculateEveningCompensation()
        {
            foreach (var dailyHour in WorkingDay.WorkingShifts)
            {
                EveningCompensation += CalculateDailyHourEveningCompensation(dailyHour);
            }

        }

        private decimal CalculateDailyHourEveningCompensation(WorkingShift dailyHour)
        {
            var eveningHours = 0M; 
            //shifts that start after midnight
            if (dailyHour.StartTime.Hour <= WagePricing.EveningPricing.EndHour)
            {
                if (dailyHour.EndTime.Hour >= WagePricing.EveningPricing.EndHour)
                {
                    eveningHours += WagePricing.EveningPricing.EndHour - (dailyHour.StartTime.Hour +
                                    (Convert.ToDecimal(dailyHour.StartTime.Minute)/60M));
                }
                else
                {
                    eveningHours += (dailyHour.EndTime.Hour + Convert.ToDecimal(dailyHour.EndTime.Minute)/60M) - (dailyHour.StartTime.Hour +
                                   (Convert.ToDecimal(dailyHour.StartTime.Minute)/60M));
                }
            }

            if (dailyHour.EndTime.Hour >= WagePricing.EveningPricing.StartHour)
            {
                if (dailyHour.StartTime.Hour <= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += (dailyHour.EndTime.Hour + Convert.ToDecimal(dailyHour.EndTime.Minute)/60M) -
                                    WagePricing.EveningPricing.StartHour;
                }
                else
                {
                    eveningHours += (dailyHour.EndTime.Hour + Convert.ToDecimal(dailyHour.EndTime.Minute)/60M) -
                                    (dailyHour.StartTime.Hour + Convert.ToDecimal(dailyHour.StartTime.Minute)/60M);
                }
            }

            //shift goes over midnight
            if (dailyHour.EndTime.Hour <= WagePricing.EveningPricing.EndHour && dailyHour.StartTime.Hour > dailyHour.EndTime.Hour)
            {
                if (dailyHour.StartTime.Hour >= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += 24 - dailyHour.StartTime.Hour - (dailyHour.StartTime.Minute/60) +
                                    dailyHour.EndTime.Hour + (dailyHour.EndTime.Minute/60);
                }
                else
                {
                    eveningHours += 24 - WagePricing.EveningPricing.StartHour - (dailyHour.StartTime.Minute / 60) +
                                    dailyHour.EndTime.Hour + (dailyHour.EndTime.Minute / 60);
                }
            }

            //shift goes over midnight and to next morning
            if (dailyHour.EndTime.Hour > WagePricing.EveningPricing.EndHour && dailyHour.StartTime.Hour > dailyHour.EndTime.Hour)
            {
                if (dailyHour.StartTime.Hour >= WagePricing.EveningPricing.StartHour)
                {
                    eveningHours += 24 - dailyHour.StartTime.Hour - (dailyHour.StartTime.Minute / 60) +
                                    WagePricing.EveningPricing.EndHour;
                }
                else
                {
                    eveningHours += 24 - WagePricing.EveningPricing.StartHour - (dailyHour.StartTime.Minute / 60) +
                                    WagePricing.EveningPricing.EndHour;
                }
            }

            if (eveningHours < 0)
            {
                throw new Exception("Evening hours should never be negative!");
            }

            EveningHours += Convert.ToDecimal(eveningHours);

            return Convert.ToDecimal(eveningHours) * WagePricing.EveningPricing.Compensation;
        }
    }
}