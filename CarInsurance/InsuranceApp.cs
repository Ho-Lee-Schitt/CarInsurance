using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DriverDetails> DriverList { get; set; }
    }
}