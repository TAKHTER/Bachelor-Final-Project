using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
       // [Required(ErrorMessage = "Teacher Name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Teacher Initial required")]
        public string Initial { get; set; }
        //[Required(ErrorMessage = "Teacher Designation required")]
        public string Designation { get; set; }
        //[Required(ErrorMessage = "Teacher Name required")]
       [ForeignKey("Department")]
        public int TeacherDeptId { set; get; }
        public virtual Department Department { set; get; }

    }
}