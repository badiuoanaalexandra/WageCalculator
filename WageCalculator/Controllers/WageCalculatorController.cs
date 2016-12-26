using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KBCsv;
using WageCalculator.Entities;
using WageCalculator.Helpers;
using WageCalculator.Models;

namespace WageCalculator.Controllers
{
    public class WageCalculatorController : BaseController
    {
        [HttpPost]
        public async Task<JsonResult> ProcessCsvFile(HttpPostedFileBase file)
        {
            try
            {
                var wageCalculatorModel = new WageCalculatorModel();
                var personDatas = await wageCalculatorModel.ProcessFile(file);
                return Json(personDatas);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }

        }
    }
}
