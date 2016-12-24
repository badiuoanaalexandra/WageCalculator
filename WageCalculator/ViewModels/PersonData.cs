using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
    public class PersonData
    {
        public long PersonID { get; set; }
        public string PersonName { get; set; }
        public MonthlyWage MonthlyWage { get; set; }
    }
}