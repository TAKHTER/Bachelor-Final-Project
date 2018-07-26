using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Semester
    {
        public int SemesterId { set; get; }
       // [Required(ErrorMessage = "Semester Name required")]
        public string Name { set; get; }
    }
}