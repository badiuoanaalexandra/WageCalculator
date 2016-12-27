using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using KBCsv;
using WageCalculator.Entities;
using WageCalculator.Helpers;
using WageCalculator.ViewModels;

namespace WageCalculator.Models
{
    /// <summary>
    /// Handles the operation from WageCalculatorController
    /// </summary>
    public class WageCalculatorModel
    {
        /// <summary>
        /// Normally these would be in a DB, but for this Demo the csv file is going to be parsed
        /// </summary>
        public List<Person> Persons => WageCalculatorHelper.Persons;

        /// <summary>
        /// Processes csv file uploaded
        /// </summary>
        /// <param name="file">HttpPostedFileBase file</param>
        /// <returns>FilterData</returns>
        public FilterData ProcessCsvFile(HttpPostedFileBase file)
        {
            WageCalculatorHelper.Persons = new List<Person>();
            var filterData = new FilterData
            {
                Months = new List<int>(),
                Years = new List<int>(),
                PersonListItems = new List<PersonListItem>()
            };

            using (var textReader = new StreamReader(file.InputStream))
            using (var reader = new CsvReader(textReader, true))
            {
                reader.ReadHeaderRecord();

                var buffer = new DataRecord[128];

                while (reader.HasMoreRecords)
                {
                    var read = reader.ReadDataRecords(buffer, 0, buffer.Length);

                    for (var i = 0; i < read; ++i)
                    {
                        ProcessCsvFileRow(buffer[i], filterData);
                    }
                }
            }
             
            return filterData;
        }

        /// <summary>
        /// Processes one row from the csv file. Same time the file is processed, filterdata is created for UI
        /// </summary>
        /// <param name="row">DataRecord</param>
        /// <param name="filterData">FilterData object</param>
        private void ProcessCsvFileRow(DataRecord row, FilterData filterData)
        {
            var personId = int.Parse(row["Person ID"].Trim());
            var person = WageCalculatorHelper.Persons.FirstOrDefault(p => p.PersonID == personId);
            if (person == null)
            {
                person = new Person
                {
                    PersonID = personId,
                    PersonName = row["Person Name"]
                };

                WageCalculatorHelper.Persons.Add(person);
                filterData.PersonListItems.Add(new PersonListItem
                {
                    PersonID = person.PersonID,
                    PersonName = person.PersonName
                });
            }

            var date = DateTime.Parse(row["Date"], new CultureInfo("fi"));
            var timeStart = row["Start"].Split(':');
            var startHour = int.Parse(timeStart[0]);
            var startMinute = int.Parse(timeStart[1]);

            var timeEnd = row["End"].Split(':');
            var endHour = int.Parse(timeEnd[0]);
            var endMinute = int.Parse(timeEnd[1]);

            var workingDay = person.WorkingDays.FirstOrDefault(o => o.Date == date);
            if (workingDay == null)
            {
                workingDay = new WorkingDay
                {
                    Date = date
                };

                person.WorkingDays.Add(workingDay);
                var month = workingDay.Date.Month;
                var year = workingDay.Date.Year;
                if (filterData.Months.All(o => o != month))
                {
                    filterData.Months.Add(month);
                }

                if (filterData.Years.All(o => o != year))
                {
                    filterData.Years.Add(year);
                }
            }

            if ((startHour < endHour) || (startHour == endHour && startMinute < endMinute))
            {
                var dailyHour = new WorkingShift
                {
                    StartTime = date.AddHours(startHour).AddMinutes(startMinute),
                    EndTime = date.AddHours(endHour).AddMinutes(endMinute)
                };

                workingDay.WorkingShifts.Add(dailyHour);
            }
            // goes over midnight -> calculated in the same day
            else
            {
                var dailyHour = new WorkingShift
                {
                    StartTime = date.AddHours(startHour).AddMinutes(startMinute),
                    EndTime = date.AddDays(1).AddHours(endHour).AddMinutes(endMinute)
                };

                workingDay.WorkingShifts.Add(dailyHour);
            }
        }

        public List<PersonData> CalculateWages(int month, int year, long personId)
        {
            var personDatas = new List<PersonData>();
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            var persons = Persons;
            if (personId > 0)
            {
                persons = Persons.Where(o => o.PersonID == personId).ToList();
            }

            foreach (var person in persons)
            {
                var filteredWorkingDays = person.WorkingDays.Where(o => o.Date.Month == month && o.Date.Year == year).ToList();
                personDatas.Add(new PersonData
                {
                    PersonID = person.PersonID,
                    PersonName = person.PersonName,
                    IntervalWage = new IntervalWageModel(filteredWorkingDays, wagePricing).CalculateIntervalWage()
                });
            }

            return personDatas;
        }
    }
}