using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    public class EveningPricing
    {
        public decimal Compensation { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }
}