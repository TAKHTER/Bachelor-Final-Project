using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class TempRoutineBackupModel
    {
        public int TempRoutineBackupModelId { set; get; }
        public string Day { set; get; }
        public string Section { set; get; }
        public string Year { set; get; }
        public DateTime Time_From { set; get; }
        public DateTime Time_To { set; get; }
        public DateTime TempRoutineDate { get; set; }

        [Required]
        public string username { set; get; }

        [ForeignKey("Course")]
        public int RoutineCourseId { set; get; }
        [ForeignKey("ClassRoom")]
        public int RoutineClassroomId { set; get; }
        [ForeignKey("Teacher")]
        public int RoutineTeacherId { set; get; }
        [ForeignKey("Semester")]
        public int RoutineSemesterId { set; get; }
        [ForeignKey("Department")]
        public int RoutineDepartmentId { set; get; }
        [ForeignKey("RoutineType")]
        public int RoutineTypeId { set; get; }
        public virtual Course Course { set; get; }
        public virtual Department Department { set; get; }
        public virtual ClassRoom ClassRoom { set; get; }
        public virtual Teacher Teacher { set; get; }
        public virtual Semester Semester { set; get; }
        public virtual RoutineType RoutineType { set; get; }

    }
}