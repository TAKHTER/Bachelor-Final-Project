using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class UserLogin
    {
        [Required]
        [Display(Name = "Role")]
        public string UserRole { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        public string Password { get; set; }

        [Display(Name="Semester")]
        public string Semester { get; set; }

        [Display(Name = "Year")]
        public string Year { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class CustomRole
    {
        public string Name { set; get; }
    }
}