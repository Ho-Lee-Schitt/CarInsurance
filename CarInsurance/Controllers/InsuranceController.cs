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
            return RedirectToAction("CalcResult", obj);
        }

        [HttpGet]
        public ActionResult CalcResult(InsuranceApp obj)
        {
            return View(obj);
        }
    }
}