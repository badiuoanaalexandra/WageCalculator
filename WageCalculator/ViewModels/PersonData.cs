using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
    /// <summary>
    /// Visualize the wage calculations for a person
    /// </summary>
    public class PersonData
    {
        public long PersonID { get; set; }
        public string PersonName { get; set; }
        public IntervalWage IntervalWage { get; set; }
    }
}