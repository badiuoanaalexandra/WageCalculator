using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WageCalculator.Entities;

namespace WageCalculator.Tests.Helpers
{
    public static class TestsHelper
    {
        public static decimal CalculateOvertime(WagePricing wagePricing, decimal overTimeHours)
        {
            var compensation = 0M;
            var hours = overTimeHours;
            foreach (var overtimePlan in wagePricing.OvertimeCompensationPlans)
            {
                compensation += CalculatePlanOvertime(wagePricing, overtimePlan, hours);
                hours -= overtimePlan.HourTimeSpan;
                if (hours < 0)
                {
                    return compensation;
                }
            }

            return compensation;
        }

        private static decimal CalculatePlanOvertime(WagePricing wagePricing,
            OvertimeCompensationPlan overtimeCompensationPlan, decimal hours)
        {
            if (hours > overtimeCompensationPlan.HourTimeSpan)
            {
                return overtimeCompensationPlan.HourTimeSpan*overtimeCompensationPlan.Percentage*
                       wagePricing.BasicHourlyWage;
            }

           return hours*overtimeCompensationPlan.Percentage*
                       wagePricing.BasicHourlyWage;
        }

        public static int CalculateShift(int startHour, int endHour)
        {
            // in case the time goes until next morning: example : 18 -> 06
            if (startHour > endHour)
            {
                return (24 - startHour) + endHour;
            }
            // in case the time ends in the same day: example 18-24
            return endHour - startHour;
        }
    }
}
