using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class InsuranceController : Controller
    {

        // GET: Insurance
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(InsuranceApp obj)
        {
            List<String> errorString = checkDrivers(obj.DriverDetails);
            if (errorString == null) {
                return RedirectToAction("CalcResult", obj);
            } else {
                ModelState.AddModelError(errorString[0], errorString[1]);
                return View(obj);
            }
        }

        [HttpGet]
        public ActionResult CalcResult(InsuranceApp obj)
        {
            return View(obj);
        }

        private List<String> checkDrivers(List<InsuranceApp.Driver> obj)
        {
            List<String> errorList = new List<String>();
            foreach (InsuranceApp.Driver carDriver in obj)
            {
                if ((checkClaims(carDriver.Claims) == true) && (String.IsNullOrEmpty(carDriver.Name) || String.IsNullOrEmpty(carDriver.Occupation) || (carDriver.DOB == default(DateTime))))
                {
                    if (String.IsNullOrEmpty(carDriver.Name)) {
                        errorList.Add("DriverName");
                        errorList.Add("Required Field.");
                    }

                    if (String.IsNullOrEmpty(carDriver.Occupation))
                    {
                        errorList.Add("DriverOcc");
                        errorList.Add("Required Field.");
                    }

                    if (carDriver.DOB == default(DateTime))
                    {
                        errorList.Add("DriverDOB");
                        errorList.Add("Required Field.");
                    }
                    return errorList;
                }


                if (!String.IsNullOrEmpty(carDriver.Name) || !String.IsNullOrEmpty(carDriver.Occupation) || (carDriver.DOB != default(DateTime)) || (checkClaims(carDriver.Claims) != false))
                {
                    break;
                }

                if (String.IsNullOrEmpty(carDriver.Name))
                {
                    errorList.Add("DriverName");
                    errorList.Add("Required Field.");
                }

                if (String.IsNullOrEmpty(carDriver.Occupation))
                {
                    errorList.Add("DriverOcc");
                    errorList.Add("Required Field.");
                }

                if (carDriver.DOB == default(DateTime))
                {
                    errorList.Add("DriverDOB");
                    errorList.Add("Required Field.");
                }
                return errorList;
            }
            //foreach (InsuranceApp.Driver carDriver in obj)
            //{
            //    if (String.IsNullOrEmpty(carDriver.Name) && String.IsNullOrEmpty(carDriver.Occupation) && (carDriver.DOB == null))
            //    {

            //    }
            //}
            return null;
        }

        private bool checkClaims(List<InsuranceApp.Driver.DateTimeWrapper> claimList)
        {
            foreach (InsuranceApp.Driver.DateTimeWrapper claimDate in claimList)
            {
                if (claimDate.ClaimDate != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}