using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageCalculator.Entities;
using WageCalculator.Helpers;

namespace WageCalculator.ViewModels
{
    /// <summary>
    /// Used for creating a selectlist
    /// </summary>
    public class PersonListItem
    {
        public long PersonID { get; set; }
        public string PersonName { get; set; }
    }
}