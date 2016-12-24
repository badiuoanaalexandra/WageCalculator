using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    public class OvertimePricing
    {
        public int ApplyOrder { get; set; }
        public decimal Percentage { get; set; }
        public int HourTimeSpan { get; set; }
    }
}