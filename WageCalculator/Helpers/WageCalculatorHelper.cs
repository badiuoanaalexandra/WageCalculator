using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.ViewModels;

namespace WageCalculator.Helpers
{
    public static class WageCalculatorHelper
    {
        public static List<Person> Persons { get; set; }

        public static WagePricing GetWagePricing()
        {
            return new WagePricing
            {
                BasicHourlyWage = 3.75M,
                BasicDayHours = 8,
                EveningPricing = new EveningPricing
                {
                    Compensation = 1.15M,
                    StartHour = 18,
                    EndHour = 6
                },
                OvertimePricings = new List<OvertimePricing>()
                {
                    new OvertimePricing
                    {
                        HourTimeSpan = 2,
                        ApplyOrder = 1,
                        Percentage = 0.25M
                    },
                     new OvertimePricing
                    {
                        HourTimeSpan = 2,
                        ApplyOrder = 2,
                        Percentage = 0.5M
                    },
                    new OvertimePricing
                    {
                        // 24 - 8 - 2 - 2
                        HourTimeSpan = 12,
                        ApplyOrder = 3,
                        Percentage = 1M
                    },
                }
            };
        }

        public static int CalculateShift(int startHour, int endHour)
        {
            // in case the time goes until next morning: example : 18 -> 06
            if (startHour > endHour)
            {
                return (24 - startHour) + endHour;
            }
            // in case the time ends in the samae day: example 18-24
            return endHour - startHour;
        }
    }
}