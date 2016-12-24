using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WageCalculator.Entities
{
    public class WagePricing
    {
        public decimal BasicHourlyWage { get; set; }
        public int BasicDayHours { get; set; }
        public EveningPricing EveningPricing { get; set; }

        public List<OvertimePricing> OvertimePricings { get; set; }
    }
}