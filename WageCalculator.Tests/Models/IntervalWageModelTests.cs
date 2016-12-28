using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WageCalculator.Entities;
using WageCalculator.Helpers;
using WageCalculator.Models;
using WageCalculator.Tests.Helpers;
using WageCalculator.ViewModels;

namespace WageCalculator.Tests.Models
{
    [TestClass]
    public class IntervalWageModelTests
    {
        /// <summary>
        /// 2 working days
        /// </summary>
        [TestMethod]
        public void Test_IntervalWage_2_Long_Days()
        {
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            //one long day
            var date = DateTime.Now.Date.AddDays(-3);
            var workingDay1 = new WorkingDay
            {
                Date = date,
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        //early morning/night
                        StartTime = date.AddHours(wagePricing.EveningPricing.EndHour - 3),
                        EndTime = date.AddHours(wagePricing.EveningPricing.EndHour - 2)
                    },
                    new WorkingShift
                    {
                        //morning
                        StartTime = date.AddHours(wagePricing.EveningPricing.EndHour - 1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.EndHour + 2)
                    },
                    new WorkingShift
                    {
                        //daytime
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour - 4),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour - 2)
                    },
                    new WorkingShift
                    {
                        //partly evening
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour - 1),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour + 2)
                    },
                    new WorkingShift
                    {
                        //evening
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour + 2).AddMinutes(30),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour + 3)
                    },
                    new WorkingShift
                    {
                        //evening over midnight
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour + 4),
                        EndTime = date.AddDays(1).AddHours(wagePricing.EveningPricing.EndHour - 3)
                    }
                }
            };

            var workingDay2 = new WorkingDay
            {
                Date = date.AddDays(2),
                WorkingShifts = new List<WorkingShift>()
                {
                    new WorkingShift
                    {
                        //daytime
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour - 4),
                        EndTime = date.AddHours(wagePricing.EveningPricing.StartHour - 1)
                    },
                    new WorkingShift
                    {
                        //evening over midnight
                        StartTime = date.AddHours(wagePricing.EveningPricing.StartHour + 1),
                        EndTime = date.AddDays(1).AddHours(wagePricing.EveningPricing.EndHour - 3)
                    }
                }
            };

            var workingDay1WorkingHours = 1 + 3 + 2 + 3 + 0.5M + TestsHelper.CalculateShift(wagePricing.EveningPricing.StartHour + 4, wagePricing.EveningPricing.EndHour - 3);
            var workingDay1NormalWage = Math.Round(workingDay1WorkingHours*wagePricing.BasicHourlyWage, 2, MidpointRounding.AwayFromZero);

            var workingDay1EveningHours = 2 + 2 + 0.5M + TestsHelper.CalculateShift(wagePricing.EveningPricing.StartHour + 4, wagePricing.EveningPricing.EndHour - 3);
            var workingDay1EveningCompensation = Math.Round(workingDay1EveningHours*wagePricing.EveningPricing.Compensation, 2, MidpointRounding.AwayFromZero);

            var workingDay1OvertimeHours = workingDay1WorkingHours - wagePricing.BasicDayHours;
            var workingDay1OvertimeCompensation = Math.Round(TestsHelper.CalculateOvertime(wagePricing, workingDay1OvertimeHours), 2, MidpointRounding.AwayFromZero);

            var workingDay2WorkingHours = 3 + TestsHelper.CalculateShift(wagePricing.EveningPricing.StartHour + 1, wagePricing.EveningPricing.EndHour - 3);
            var workingDay2NormalWage = Math.Round(workingDay2WorkingHours*wagePricing.BasicHourlyWage, 2, MidpointRounding.AwayFromZero);

            var workingDay2EveningHours = TestsHelper.CalculateShift(wagePricing.EveningPricing.StartHour + 1, wagePricing.EveningPricing.EndHour - 3);
            var workingDay2EveningCompensation = Math.Round(workingDay2EveningHours*wagePricing.EveningPricing.Compensation, 2, MidpointRounding.AwayFromZero);

            var workingDay2OvertimeHours = workingDay2WorkingHours - wagePricing.BasicDayHours;
            var workingDay2OvertimeCompensation = Math.Round(TestsHelper.CalculateOvertime(wagePricing, workingDay2OvertimeHours), 2, MidpointRounding.AwayFromZero);

            var workingDays = new List<WorkingDay>()
            {
                workingDay1,
                workingDay2
            };
            var intervalWage = new IntervalWageModel(workingDays, wagePricing).CalculateIntervalWage();

            var totalWage = workingDay1NormalWage + workingDay2NormalWage +
                            workingDay1EveningCompensation + workingDay2EveningCompensation +
                            workingDay1OvertimeCompensation + workingDay2OvertimeCompensation;
            Assert.AreEqual(workingDay1WorkingHours + workingDay2WorkingHours, intervalWage.TotalWorktimeHours);
            Assert.AreEqual(totalWage, intervalWage.TotalWage);

            Assert.AreEqual(workingDay1EveningHours + workingDay2EveningHours, intervalWage.TotalEveningHours);
            Assert.AreEqual(workingDay1EveningCompensation + workingDay2EveningCompensation,
                intervalWage.TotalEveningCompensation);

            Assert.AreEqual(workingDay1OvertimeHours + workingDay2OvertimeHours, intervalWage.TotalOvertimeHours);
            Assert.AreEqual(workingDay1OvertimeCompensation + workingDay2OvertimeCompensation,
                intervalWage.TotalOvertimeCompensation);
        }
    }
}
