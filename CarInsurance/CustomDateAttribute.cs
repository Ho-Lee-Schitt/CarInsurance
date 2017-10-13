using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInsurance
{
    public class CustomDateAttribute : RangeAttribute
    {
        public CustomDateAttribute()
          : base(typeof(DateTime),
                  DateTime.Now.ToShortDateString(),
                  DateTime.Now.AddYears(1).ToShortDateString())
        { }
    }
}
