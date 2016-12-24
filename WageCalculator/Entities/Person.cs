using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    public class Person
    {
        public Person()
        {
            WorkingDays = new List<WorkingDay>();
        }

        public long PersonID { get; set; }
        public string PersonName { get; set; }
        public List<WorkingDay> WorkingDays { get; set; }
    }
}