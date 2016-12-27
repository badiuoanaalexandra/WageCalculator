using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
    /// <summary>
    /// Filter data based on the csv file
    /// </summary>
    public class FilterData
    {
        public List<int> Years { get; set; }
        public List<int> Months { get; set; }
        public Dictionary<long, string> PersonNames { get; set; }
    }
}