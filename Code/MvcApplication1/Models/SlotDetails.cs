using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class MasterRoutine 
    {
        public string Day { set; get; }
        public string[] Time { set; get; }
        public string[] Slot { set; get; }
        public MasterRoutine() 
        {
            Time = new string[6];
            Slot = new string[18];
        } 
    }
    public class SlotDetailsEve 
    {
        public string RoomNo { set; get; }
        public string CourseCode { set; get; }
        public string CourseName { set; get; }
        public string BatchNumber { set; get; }
        public string TeacherInitial { set; get; }

    }
    public class TimeInfoEve
    {
        public DateTime TimeFrom { set; get; }
        public DateTime TimeTo { set; get; }
        public string aString { set; get; }
        public List<SlotDetailsEve> slotDetailsList { set; get; }
        public TimeInfoEve()
        {
            aString = "-";
            slotDetailsList = new List<SlotDetailsEve>();
        }
    }
    public class RoutineViewEve
    {
        public string Day { set; get; }
        public List<TimeInfoEve> aRoutineRow { set; get; }
        public RoutineViewEve()
        {
            aRoutineRow = new List<TimeInfoEve>();
        }
    }


    public class SlotDetails
    {
        public string RoomNo { set; get;}
        public string CourseCode { set; get; }
        public string TeacherInitial { set; get; }
    }

    public class TimeInfo
    {
        public DateTime TimeFrom { set; get; }
        public DateTime TimeTo { set; get; }
        public string aString { set; get; }
        public List<SlotDetails> slotDetailsList { set; get; }
        public TimeInfo() 
        {
            aString = "-";
            slotDetailsList = new List<SlotDetails>();
        }
    }
    public class RoutineView
    {
        public string Day { set; get; }
        public List<TimeInfo> aRoutineRow { set; get; }
        public RoutineView()
        {
            aRoutineRow = new List<TimeInfo>();
        }
    }
    

}