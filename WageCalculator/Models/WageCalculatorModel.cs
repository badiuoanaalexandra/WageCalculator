using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using KBCsv;
using WageCalculator.Entities;
using WageCalculator.Helpers;
using WageCalculator.ViewModels;

namespace WageCalculator.Models
{
    public class WageCalculatorModel
    {
        private readonly List<Person> _persons = new List<Person>();

        public async Task<List<PersonData>> ProcessFile(HttpPostedFileBase file)
        {
            _persons.Clear();
            using (var textReader = new StreamReader(file.InputStream))
            using (var reader = new CsvReader(textReader, true))
            {
                await reader.ReadHeaderRecordAsync();

                var buffer = new DataRecord[128];

                while (reader.HasMoreRecords)
                {
                    var read = await reader.ReadDataRecordsAsync(buffer, 0, buffer.Length);

                    for (var i = 0; i < read; ++i)
                    {
                        ProcessCsvFileRow(buffer[i]);
                    }
                }
            }

            return CalculateWages(3);
        }

        private void ProcessCsvFileRow(DataRecord row)
        {
            var personId = int.Parse(row["Person ID"].Trim());
            var person = _persons.FirstOrDefault(p => p.PersonID == personId);
            if (person == null)
            {
                person = new Person
                {
                    PersonID = personId,
                    PersonName = row["Person Name"]
                };

                _persons.Add(person);
            }

            var date = DateTime.Parse(row["Date"]);
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
            }

            if ((startHour < endHour) || (startHour == endHour && startMinute < endMinute))
            {
                var dailyHour = new WorkingShift
                {
                    StartTime = date.AddHours(startHour).AddMinutes(startMinute),
                    EndTime = date.AddHours(endHour).AddMinutes(endMinute)
                };

                workingDay.DailyHours.Add(dailyHour);
            }
            // goes over midnight -> calculated in the same day
            else
            {
                var dailyHour = new WorkingShift
                {
                    StartTime = date.AddHours(startHour).AddMinutes(startMinute),
                    EndTime = date.AddDays(1).AddHours(endHour).AddMinutes(endMinute)
                };

                workingDay.DailyHours.Add(dailyHour);
            }
        }

        private List<PersonData> CalculateWages(int month)
        {
            var personDatas = new List<PersonData>();
            var wagePricing = WageCalculatorHelper.GetWagePricing();
            foreach (var person in _persons)
            {
                var monthWorkingDays = person.WorkingDays.Where(o => o.Date.Month == month).ToList();
                personDatas.Add(new PersonData
                {
                    PersonID = person.PersonID,
                    PersonName = person.PersonName,
                    MonthlyWage = new MonthlyWage(monthWorkingDays, wagePricing)
                });
            }

            return personDatas;
        }
    }
}