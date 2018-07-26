using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Department
    {
        public int DepartmentId { set; get; }
        //[Required(ErrorMessage = "Department Name required")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Department Code required")]
        public string Code { set; get; }
        public virtual List<Teacher> Teacher { set; get; }
        public virtual List<Course> Course { set; get; }
    }
}