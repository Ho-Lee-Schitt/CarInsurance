using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarInsurance
{
    public class DriverDetails
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

        [Display(Name = "Claim Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime[] Claims { get; set; }
}
}