using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;


namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {

         [Authorize(Roles = "Admin")]
        public ActionResult ManageOthers()
        {
            return View();
        }
      
        private AllEntitiesContext db = new AllEntitiesContext();
        public ActionResult Index(string DepartmentId, string TypeId, string SemesterId, string Year)
        {
            try
            {
                List<RoutineView> RoutineViewList = new List<RoutineView>();
                List<RoutineViewEve> RoutineViewListEve = new List<RoutineViewEve>();
                RoutineViewHelper aRoutineViewHelper = new RoutineViewHelper();
                ViewBag.Message = "Welcome to Daffodil International University's Routine Management System";
                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
                ViewBag.Year = new SelectList(db.Routines.Select(a => a.Year).Distinct());
                ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
//                ViewBag.TypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName");
 
                List<RoutineType> aList = new List<RoutineType>();
                List<RoutineType> bList = new List<RoutineType>();
                aList=db.RoutineTypes.ToList();
                foreach (var a in aList) 
                {
                    
                    if (a.TypeName != "Temporary") 
                    {
                        bList.Add(a);
                    }
                }
                //ViewBag.TypeId = new SelectList(aList,"RoutineTypeId",""
                ViewBag.TypeId = new SelectList(bList, "RoutineTypeId", "TypeName");
                
                if ((Year == null || Year == "") || (DepartmentId == null || DepartmentId == "") || (TypeId == null || TypeId == "") || (SemesterId == null || SemesterId == ""))
                {
                    ViewBag.Message2 = "None of the Fields can be empty";
                    return View();
                }
                int id = Convert.ToInt32(TypeId);
                int semId = Convert.ToInt32(SemesterId);
                int deptId = Convert.ToInt32(DepartmentId);
                string TypeName = db.RoutineTypes.Where(a => a.RoutineTypeId == id).Select(a => a.TypeName).FirstOrDefault();
                string deptname = db.Departments.Where(a => a.DepartmentId == deptId).Select(a => a.Name).FirstOrDefault();
                string semestername = db.Semesters.Where(a => a.SemesterId == semId).Select(a => a.Name).FirstOrDefault();
                
                var routines = db.Routines.Where(r => r.RoutineType.TypeName == TypeName).Include(r => r.Course).Include(r => r.ClassRoom).Include(r => r.Teacher).Include(r => r.Semester).Include(r => r.RoutineType);
                routines = routines.Where(a => a.Year == Year).Where(a => a.Course.Department.DepartmentId == deptId).Where(a => a.Semester.SemesterId == semId);

                var routinesEve = db.EveRoutines.Where(r => r.RoutineType.TypeName == TypeName).Include(r => r.Course).Include(r => r.ClassRoom).Include(r => r.Teacher).Include(r => r.Semester).Include(r => r.RoutineType);
                routinesEve = routinesEve.Where(a => a.Year == Year).Where(a => a.Course.Department.DepartmentId == deptId).Where(a => a.Semester.SemesterId == semId);


                if (TypeName == "Temporary") 
                {
                    ViewBag.NotFoundMessage = "Temporary routine does not have a Master View";
                    ViewBag.Message2 = "None of the Fields can be empty";
                    return View();
                }


                if (TypeName == "Day")
                {
                    if (routines.Count() == 0)
                    {
                        ViewBag.NotFoundMessage = "The routine you are searching for is not available yet";
                        ViewBag.Message2 = "None of the Fields can be empty";
                        return View();
                    }
                    List<DateTime> DayTimeList = new List<DateTime>();
                    
                    //Dynamically Gets Time of a routine

                    List<DateTime> DayTimeList2 = new List<DateTime>();
                    DayTimeList = db.Routines.Where(a => a.RoutineType.TypeName == "Day").Select(a => a.Time_From).Distinct().ToList();
                    DayTimeList2 = db.Routines.Where(a => a.RoutineType.TypeName == "Day").Select(a => a.Time_To).Distinct().ToList();
                    //DayTimeList.AddRange(DayTimeList2);
                    //DayTimeList.Union(DayTimeList2);
                    //DayTimeList.Intersect(DayTimeList2);
                    foreach (DateTime a in DayTimeList2)
                    {
                        // DateTime n=DayTimeList.Where(b=>b.TimeOfDay!=a.TimeOfDay).Select(b=>b).FirstOrDefault();
                        if (DayTimeList.Contains(a) == false)
                        {
                            DayTimeList.Add(a);
                        }
                    }
                    //DayTimeList.Add(Convert.ToDateTime("08:30"));
                    //DayTimeList.Add(Convert.ToDateTime("10:00"));
                    //DayTimeList.Add(Convert.ToDateTime("11:30"));
                    //DayTimeList.Add(Convert.ToDateTime("01:00"));
                    //DayTimeList.Add(Convert.ToDateTime("02:30"));
                    //DayTimeList.Add(Convert.ToDateTime("04:00"));
                    //DayTimeList.Add(Convert.ToDateTime("05:30"));
                    //DayTimeList.Distinct();
                    RoutineViewList = aRoutineViewHelper.GetRoutineViewList(routines.ToList(), DayTimeList);
                    // return View("RView", RoutineViewList);
                    
                    ViewBag.Message = "Class Routine For " + deptname + " Department"+" ( "+TypeName+" )"+ " Program.";
                    ViewBag.Message1 = " ********************************************** For Semester "+ semestername + "' "+Year+" *****************************************";
                    return View("RView",RoutineViewList);
                }
                if (TypeName == "Evening")
                {
                    if (routinesEve.Count() == 0)
                    {
                        ViewBag.NotFoundMessage = "The routine you are searching for is not available yet";
                        ViewBag.Message2 = "None of the Fields can be empty";
                        return View();
                    }
                    List<DateTime> DayTimeList = new List<DateTime>();
                    
                    //
                    //Dynamically gets time

                    List<DateTime> DayTimeList2 = new List<DateTime>();
                    DayTimeList = db.Routines.Where(a => a.RoutineType.TypeName == "Evening").Select(a => a.Time_From).Distinct().ToList();
                    DayTimeList2 = db.Routines.Where(a => a.RoutineType.TypeName == "Evening").Select(a => a.Time_To).Distinct().ToList();
                    foreach (DateTime a in DayTimeList2)
                    {
                        if (DayTimeList.Contains(a) == false)
                        {
                            DayTimeList.Add(a);
                        }
                    }

                    //DayTimeList.Add(Convert.ToDateTime("03:00"));
                    //DayTimeList.Add(Convert.ToDateTime("06:00"));
                    //DayTimeList.Add(Convert.ToDateTime("07:30"));
                    //DayTimeList.Add(Convert.ToDateTime("09:00"));
                    //RoutineViewList = aRoutineViewHelper.GetRoutineViewListEve(routines.ToList(), DayTimeList);
                    RoutineViewListEve = aRoutineViewHelper.GetRoutineViewListEve(routinesEve.ToList());

                    ViewBag.Message = "Class Routine For " + deptname + " Department" + " ( " + TypeName + " )" + " Program.";
                    ViewBag.Message1 = " ********************************************** For Semester " + semestername + "' " + Year + " ***********************************************";
                    return View("EView", RoutineViewListEve);
                    // return View("RView", RoutineViewList);
                }

                return View(RoutineViewList);
                //return View(routines);
            }
            catch
            {
                ViewBag.M = "Unable to load Index HomeController";
                return View();
            }

        }
        
        public ActionResult About()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load About HomeController";
                return View();
            }
        }
    }
}
