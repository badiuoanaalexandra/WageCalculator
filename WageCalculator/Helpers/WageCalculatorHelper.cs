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

        /// <summary>
        /// This can change and the program will still calculate wages successfully
        /// </summary>
        /// <returns></returns>
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
                OvertimeCompensationPlans = new List<OvertimeCompensationPlan>()
                {
                    new OvertimeCompensationPlan
                    {
                        HourTimeSpan = 2,
                        ApplyOrder = 1,
                        Percentage = 0.25M
                    },
                     new OvertimeCompensationPlan
                    {
                        HourTimeSpan = 2,
                        ApplyOrder = 2,
                        Percentage = 0.5M
                    },
                    new OvertimeCompensationPlan
                    {
                        // 24 - 8 - 2 - 2
                        HourTimeSpan = 12,
                        ApplyOrder = 3,
                        Percentage = 1M
                    },
                }
            };
        }
    }
}