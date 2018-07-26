using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class RoutineViewHelper
    {
        public List<string> DayList { set; get; }
        
        public RoutineViewHelper() 
        {
            DayList = new List<string>();
            DayList.Add("Saturday");
            DayList.Add("Sunday");
            DayList.Add("Monday");
            DayList.Add("Tuesday");
            DayList.Add("Wednesday");
            DayList.Add("Thursday");
            DayList.Add("Friday");
        }
        public List<MasterRoutine> GetMasterRoutine(List<Routine> routineList, List<DateTime> TimeList) 
        {
            List<MasterRoutine> MasterRoutineList = new List<MasterRoutine>();
            foreach (var d in DayList) 
            {
                MasterRoutine aMasterRoutine = new MasterRoutine();
                aMasterRoutine.Day = d;
                for (int i=0;i<(TimeList.Count-1);i++) 
                {
                    aMasterRoutine.Time[i] = TimeList[i].ToString() + "-" + TimeList[i + 1].ToString();
                }

            }
            return MasterRoutineList;
        }

        public List<RoutineView> GetRoutineViewList(List<Routine> routineList, List<DateTime> TimeList) 
        {
            List<RoutineView> aRoutineViewList = new List<RoutineView>();
            
            List<Routine> Routine1 = new List<Routine>();
            List<Routine> Routine2 = new List<Routine>();
            
            RoutineView routineView1 = new RoutineView();
            foreach (var d in DayList)
            {
                RoutineView aRoutineView = new RoutineView();
                aRoutineView.Day = d;
                Routine1 = routineList.Where(a => a.Day == d).ToList();
                int timeCount = TimeList.Count - 1;
                for (int i = 0; i < timeCount; i++)
                {
                    TimeInfo aSingleTimeInfo = new TimeInfo();
                    Routine2 = Routine1.Where(a => a.Time_From.TimeOfDay == TimeList[i].TimeOfDay).ToList();
                    aSingleTimeInfo.TimeFrom = TimeList[i];
                    aSingleTimeInfo.aString = "-";
                    aSingleTimeInfo.TimeTo = TimeList[i + 1];
                    foreach (var a in Routine2)
                    {
                        SlotDetails aSingleSlotDetails = new SlotDetails();
                        string code = null ;
                        if (a.Section!=null)
                        {
                            code = a.Course.Code.ToString() + " (" + a.Section.ToString() + ")";
                        }
                        else 
                        {
                            code = a.Course.Code;
                        }
                        aSingleSlotDetails.CourseCode = code;
                        aSingleSlotDetails.RoomNo = a.ClassRoom.RoomNo;
                        aSingleSlotDetails.TeacherInitial = a.Teacher.Initial;
                        aSingleTimeInfo.slotDetailsList.Add(aSingleSlotDetails);


                    }
                    aRoutineView.aRoutineRow.Add(aSingleTimeInfo);
                }

                aRoutineViewList.Add(aRoutineView);
            }
            return aRoutineViewList;
        }

        //
        //Pore falaia dibo..............

        public List<RoutineViewEve> GetRoutineViewListEve(List<EveRoutine> routineList)
        {
            //List<DateTime> TimeList= new List<DateTime>();
            List<RoutineViewEve> aRoutineViewList = new List<RoutineViewEve>();

            List<EveRoutine> Routine1 = new List<EveRoutine>();
            List<EveRoutine> Routine2 = new List<EveRoutine>();

            RoutineViewEve routineView1 = new RoutineViewEve();
            foreach (var d in DayList)
            {
                List<DateTime> TimeList= new List<DateTime>();
                if (d != "Friday")
                {
                    //TimeList.Add(Convert.ToDateTime("03:00"));
                    TimeList.Add(Convert.ToDateTime("18:00"));
                    TimeList.Add(Convert.ToDateTime("19:30"));
                    TimeList.Add(Convert.ToDateTime("21:00"));
                }
                else 
                {
                    TimeList.Add(Convert.ToDateTime("15:00"));
                    TimeList.Add(Convert.ToDateTime("18:00"));
                    //TimeList.Add(Convert.ToDateTime("07:30"));
                    TimeList.Add(Convert.ToDateTime("21:00"));
                }
                RoutineViewEve aRoutineView = new RoutineViewEve();
                aRoutineView.Day = d;
                Routine1 = routineList.Where(a => a.Day == d).ToList();
                int timeCount = TimeList.Count - 1;
                for (int i = 0; i < timeCount; i++)
                {
                    TimeInfoEve aSingleTimeInfo = new TimeInfoEve();
                    Routine2 = Routine1.Where(a => a.Time_From.TimeOfDay == TimeList[i].TimeOfDay).ToList();
                    aSingleTimeInfo.TimeFrom = TimeList[i];
                    aSingleTimeInfo.aString = "-";
                    aSingleTimeInfo.TimeTo = TimeList[i + 1];
                    foreach (var a in Routine2)
                    {
                        SlotDetailsEve aSingleSlotDetails = new SlotDetailsEve();
                        string code = null;
                        if (a.Section != null)
                        {
                            code = a.Course.Code.ToString() + " (" + a.Section.ToString() + ")";
                        }
                        else
                        {
                            code = a.Course.Code;
                        }
                        aSingleSlotDetails.CourseCode = code;
                        aSingleSlotDetails.RoomNo = a.ClassRoom.RoomNo;
                        aSingleSlotDetails.TeacherInitial = a.Teacher.Initial;
                        aSingleSlotDetails.BatchNumber = a.BatchNumber.ToString();
                        aSingleSlotDetails.CourseName = a.Course.Name;
                        aSingleTimeInfo.slotDetailsList.Add(aSingleSlotDetails);


                    }
                    aRoutineView.aRoutineRow.Add(aSingleTimeInfo);
                }

                aRoutineViewList.Add(aRoutineView);
            }
            return aRoutineViewList;
        }



    }
}