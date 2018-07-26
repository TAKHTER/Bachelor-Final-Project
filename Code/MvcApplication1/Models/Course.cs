using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Course
    {
        public int CourseId { set; get; }
        
       // [Required(ErrorMessage = "Course Name required")]
        public string Name { set; get; }
        
        [Required(ErrorMessage = "Course Code required")]
        public string Code { set; get; }
        
        //[Required(ErrorMessage = "Course Credit required")]
       // [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",ErrorMessage = "Numbers and special characters are not allowed in the last name.")]
        public string Credit { set; get; }
        
        //[Required(ErrorMessage = "Course Level required")]
        public string Level { set; get; }
        
        //[Required(ErrorMessage = "Course Term required")]
        public string Term { set; get; }
        [ForeignKey("Department")]
        //[Required(ErrorMessage = "Dept Name required")]
        public int CourseDeptId { set; get; }
        public virtual Department Department { set; get; }
        
    }
}