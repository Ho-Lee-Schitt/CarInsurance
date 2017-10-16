using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class InsuranceController : Controller
    {

        InsuranceApp insuranceClaim = null;

        // GET: Insurance
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Returns the create view.
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: Handles the incomming POST response from the form.
        // 
        // This will check for any errors in the Policy and only continue if it is valid.
        [HttpPost]
        public ActionResult Create(InsuranceApp obj)
        {
            String errorString = checkDrivers(obj.DriverDetails);
            if (String.IsNullOrEmpty(errorString))
            {
                if (obj.PolStartDate == null)
                {
                    errorString = "Policy Start date incorrect.";
                    goto failed;
                }
                else if (obj.PolStartDate < DateTime.Now.Date)
                {
                    errorString = "Start Date of Policy.";
                    goto failed;
                }
                TempData["insuranceClaim"] = obj;
                return RedirectToAction("CalcResult");
            }

failed:
            ModelState.AddModelError("TestError", errorString);
            return View(obj);
            
        }

        // GET: Calculates the Price of the Policy.
        [HttpGet]
        public ActionResult CalcResult()
        {
            InsuranceApp obj = (InsuranceApp)TempData["insuranceClaim"];
            if (obj != null)
            {
                double premimumPrice = 500;
                int ageOfYoungest = 1000;
                bool chauDriver = false;
                bool countDriver = false;
                int polClaims = 0;
                bool declined = false;
                int countNumDrivers = 0;

                foreach (InsuranceApp.Driver carDriver in obj.DriverDetails)
                {
                    if (!String.IsNullOrEmpty(carDriver.Name))
                    {
                        int driverClaims = 0;
                        countNumDrivers++;

                        if (CalculateAge(carDriver.DOB) < 21 || CalculateAge(carDriver.DOB) > 75)
                        {
                            ModelState.AddModelError("TestError", "Age of " + (CalculateAge(carDriver.DOB) < 21 ? "Youngest" : "Oldest") + " Driver.");
                            declined = true;
                        }


                        if ((String.Compare(carDriver.Occupation, "Chauffer") == 0) && (chauDriver == false))
                        {
                            premimumPrice *= 1.1;
                            chauDriver = true;
                        }
                        if ((String.Compare(carDriver.Occupation, "Accountant") == 0) && (countDriver == false))
                        {
                            premimumPrice *= 0.9;
                            countDriver = true;
                        }
                        if (CalculateAge(carDriver.DOB) < ageOfYoungest)
                        {
                            ageOfYoungest = CalculateAge(carDriver.DOB);
                        }

                        foreach (InsuranceApp.Driver.DateTimeWrapper claimDate in carDriver.Claims)
                        {
                            if (claimDate.ClaimDate != null)
                            {
                                polClaims++;
                                if (polClaims > 3)
                                {
                                    ModelState.AddModelError("TestError", "Policy has more than 3 claims.");
                                    declined = true;
                                }
                                driverClaims++;
                                if (driverClaims > 2)
                                {
                                    ModelState.AddModelError("TestError", "Driver " + carDriver.Name + " has more than 2 claims.");
                                    declined = true;
                                }

                                if ((claimDate.ClaimDate - obj.PolStartDate) < new TimeSpan(365, 0, 0, 0, 0))
                                {
                                    premimumPrice *= 1.2;
                                }
                                if (((claimDate.ClaimDate - obj.PolStartDate) < new TimeSpan((365 * 5), 0, 0, 0, 0)) && ((claimDate.ClaimDate - obj.PolStartDate) >= new TimeSpan((365 * 2), 0, 0, 0, 0)))
                                {
                                    premimumPrice *= 1.1;
                                }
                            }
                        }
                        if (ageOfYoungest >= 21 && ageOfYoungest <= 25)
                        {
                            premimumPrice *= 1.2;
                        }
                        if (ageOfYoungest >= 26 && ageOfYoungest <= 75)
                        {
                            premimumPrice *= 0.9;
                        }
                    }
                }
                obj.PremimumPrice = ((declined == true) ? 0 : premimumPrice);
                ViewBag.TotalDrivers = countNumDrivers;
                return View(obj);
            }
            obj = new InsuranceApp();
            obj.PremimumPrice = 0;
            return View(obj);
        }

        // Function to check if the list of drivers is valid.
        private String checkDrivers(List<InsuranceApp.Driver> obj)
        {
            bool haveDriver = false;
            foreach (InsuranceApp.Driver carDriver in obj)
            {
                if ((checkClaims(carDriver.Claims) == true) && (String.IsNullOrEmpty(carDriver.Name) || String.IsNullOrEmpty(carDriver.Occupation) || (carDriver.DOB == default(DateTime))))
                {
                    return "Driver details are required.";
                }


                if (!String.IsNullOrEmpty(carDriver.Name) && !String.IsNullOrEmpty(carDriver.Occupation) && (carDriver.DOB != default(DateTime)))
                {
                    haveDriver = true;
                    if (checkClaimAge(carDriver.Claims) == false)
                    {
                        return "Claim date invalid.";
                    }
                    continue;
                }
            }

            if (!haveDriver)
            {
                return "You need to enter at least one driver.";
            }
            else
            {
                return null;
            }
            //foreach (InsuranceApp.Driver carDriver in obj)
            //{
            //    if (String.IsNullOrEmpty(carDriver.Name) && String.IsNullOrEmpty(carDriver.Occupation) && (carDriver.DOB == null))
            //    {

            //    }
            //}
        }

        // Function to check if the list of claims is valid.
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

        // Function to check if the list of claims is is less that today.
        private bool checkClaimAge(List<InsuranceApp.Driver.DateTimeWrapper> claimList)
        {
            foreach (InsuranceApp.Driver.DateTimeWrapper claimDate in claimList)
            {
                if (claimDate.ClaimDate >= DateTime.Now)
                {
                    return false;
                }
            }
            return true;
        }

        // Function to calculate driver's age.
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}