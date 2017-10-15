using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance
{
    public class InsuranceApp
    {
        [Required(ErrorMessage = "Appointment Time is a required field")]
        [Display(Name = "Appointment Time")]
        [DataType(DataType.Date)]
        [CustomDateAttribute]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PolStartDate { get; set; }

        public struct Driver
        {
            [Required(ErrorMessage = "Driver Name is a required field")]
            [Display(Name = "Driver Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Occupation is a required field")]
            [Display(Name = "Occupation")]
            public string Occupation { get; set; }

            [Required(ErrorMessage = "Date Of Birth is a required field")]
            [Display(Name = "Date Of Birth")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime DOB { get; set; }

            public class DateTimeWrapper
            {
                [Display(Name = "Claim Date")]
                [DataType(DataType.Date)]
                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                public DateTime? ClaimDate { get; set; }
            }

            [Display(Name = "Claim Date")]
            public List<DateTimeWrapper> Claims { get; set; }
        }

        [Required(ErrorMessage = "Driver is a required field")]
        [Display(Name = "Driver")]
        public List<Driver> DriverDetails { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Premimum Price")]
        public double PremimumPrice { get; set; }
    }
}