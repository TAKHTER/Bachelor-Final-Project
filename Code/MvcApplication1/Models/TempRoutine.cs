using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class TempRoutine:Routine
    {
//        [DataType(DataType.Date)]
        public DateTime TempRoutineDate { get; set; }
        [Required]
        public string UserName { get; set; }

    }
}