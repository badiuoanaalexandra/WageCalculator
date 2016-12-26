using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WageCalculator.Entities;
using WageCalculator.Helpers;
using WageCalculator.ViewModels;

namespace WageCalculator.Tests.ViewModels
{
    [TestClass]
    public class DailyWageTests
    {
        /// <summary>
        /// Working day between normal hours
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_During_Normal_Hours()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //one normal day
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(6),
                        EndTime = date.AddHours(6).AddHours(wagePricing.BasicDayHours)
                    }
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);

            Assert.AreEqual(wagePricing.BasicDayHours*wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual(wagePricing.BasicDayHours, dailyWage.WorkingHours);
            Assert.AreEqual(wagePricing.BasicDayHours * wagePricing.BasicHourlyWage, dailyWage.TotalWage);
        }

        /// <summary>
        /// Working day during normal hours, but in multiple shifts
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_During_Normal_Hours_with_Multiple_Shifts()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(7),
                        EndTime = date.AddHours(10)
                    },
                    new WorkingShift
                    {
                        StartTime = date.AddHours(11),
                        EndTime = date.AddHours(14)
                    },
                    new WorkingShift
                    {
                        StartTime = date.AddHours(16).AddMinutes(15),
                        EndTime = date.AddHours(18)
                    }
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);

            Assert.AreEqual( (3 + 3+ 1.75M) * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual((3 + 3 + 1.75M), dailyWage.WorkingHours);
        }

        /// <summary>
        /// Working day during evening time
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_During_Evening_Hours()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //day starts after evening hour
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour+1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour+5)
                    },
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);
            Assert.AreEqual(4 * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual(4, dailyWage.WorkingHours);
            Assert.AreEqual(4 * wagePricing.EveningPricing.Compensation, dailyWage.EveningCompensation);
            Assert.AreEqual(4, dailyWage.EveningHours);
        }

        /// <summary>
        /// Working day starts before evening hours and continues during evening hours
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_With_Working_Shift_Starting_Before_Evening_Hours()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //day starts before evening hours and ends during evening hours
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour-1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour+5)
                    },
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);
            Assert.AreEqual(6 * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual(6, dailyWage.WorkingHours);
            Assert.AreEqual(5 * wagePricing.EveningPricing.Compensation, dailyWage.EveningCompensation);
            Assert.AreEqual(5, dailyWage.EveningHours);
        }

        /// <summary>
        /// Working day starts during evening hours and ends in the morning during normal hours
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_With_Working_Shift_Starting_During_Evening_Hours_Ending_Normal_Hours()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //day starts after evening hours and ends in the morning when evening hours has passed
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour+4),
                        EndTime = date.AddDays(1).AddHours(wagePricing.EveningPricing.EndHour+1)
                    },
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);
            Assert.AreEqual(9 * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual(9, dailyWage.WorkingHours);
            Assert.AreEqual(8 * wagePricing.EveningPricing.Compensation, dailyWage.EveningCompensation);
            Assert.AreEqual(8, dailyWage.EveningHours);
            Assert.AreEqual(1, dailyWage.OvertimeHours);
            Assert.AreEqual(1*wagePricing.BasicHourlyWage*wagePricing.OvertimePricings[0].Percentage, dailyWage.OvertimeCompensation);
        }

        /// <summary>
        /// Working day starts during normal hours, continues during the evening hours and ends during normal hours 
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_Working_Shift_All_Hours()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour-1),
                        EndTime = date.AddDays(1).AddHours(wagePricing.EveningPricing.EndHour+1)
                    },
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);
            Assert.AreEqual(14 * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual(14, dailyWage.WorkingHours);
            Assert.AreEqual(12 * wagePricing.EveningPricing.Compensation, dailyWage.EveningCompensation);
            Assert.AreEqual(12, dailyWage.EveningHours);
            Assert.AreEqual(1.875M + 3.75M + 7.5M, dailyWage.OvertimeCompensation);
        }

        /// <summary>
        /// Tests if person has normal time, evening time and overtime with multiple shifts
        /// </summary>
        [TestMethod]
        public void Test_DailyWage_Multiple_Shifts_And_Overtime()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //one normal day
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                     new WorkingShift
                    {
                        //early morning/night
                        StartTime = date.AddHours(wagePricing.EveningPricing.EndHour-3),
                        EndTime = date.AddHours(wagePricing.EveningPricing.EndHour-2)
                    },
                    new WorkingShift
                    {
                        //morning
                        StartTime = date.AddHours(wagePricing.EveningPricing.EndHour-1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.EndHour+2)
                    },
                    new WorkingShift
                    {   //daytime
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour-4),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour-2)
                    },
                    new WorkingShift
                    {
                        //partly evening
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour-1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour+2)
                    },
                     new WorkingShift
                    {
                        //evening
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour+2).AddMinutes(30),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour+3)
                    },
                       new WorkingShift
                    {
                        //evening over midnight
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour+4),
                        EndTime = date.AddDays(1).AddHours(wagePricing.EveningPricing.EndHour-3)
                    }
                }
            };

            var dailyWage = new DailyWage(workingDay, wagePricing);
            Assert.AreEqual((1+3+2+3+0.5M+5) * wagePricing.BasicHourlyWage, dailyWage.NormalHoursWage);
            Assert.AreEqual((1+3 + 2 + 3 + 0.5M + 5), dailyWage.WorkingHours);
            Assert.AreEqual(Math.Round(9.5M * wagePricing.EveningPricing.Compensation, 2, MidpointRounding.AwayFromZero), dailyWage.EveningCompensation);
            Assert.AreEqual(9.5M, dailyWage.EveningHours);
            Assert.AreEqual(1.875M + 3.75M + 9.375M, dailyWage.OvertimeCompensation);
            Assert.AreEqual(6.5M, dailyWage.OvertimeHours);
        }
    }
}
