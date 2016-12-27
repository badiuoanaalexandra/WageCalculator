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
    /// <summary>
    /// Controller for WageCalculator
    /// </summary>
    public class WageCalculatorController : BaseController
    {
        /// <summary>
        /// For this Demo WageCalculatorModel will hold the data from the csv file
        /// </summary>
        WageCalculatorModel _wageCalculatorModel = new WageCalculatorModel();

        /// <summary>
        /// Processes file uploaded by user
        /// </summary>
        /// <param name="file">HttpPostedFileBase file</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public async Task<JsonResult> ProcessCsvFile(HttpPostedFileBase file)
        {
            try
            {
                var personDatas = await _wageCalculatorModel.ProcessCsvFile(file);
                return Json(personDatas);
            }
            catch(Exception ex)
            {
                //here we would have some tracing
                return Json(ex.Message);
            }

        }

        /// <summary>
        /// Calculates wages based on filter
        /// </summary>
        /// <param name="month">Month</param>
        /// <param name="year">Year</param>
        /// <param name="personId">Year</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult CalculateWage(int month, int year, long personId)
        {
            try
            {
                var personDatas = _wageCalculatorModel.CalculateWages(month, year, personId);
                return Json(personDatas);
            }
            catch (Exception ex)
            {
                //here we would have some tracing
                return Json(ex.Message);
            }

        }
    }
}
