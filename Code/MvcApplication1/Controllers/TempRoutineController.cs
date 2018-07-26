using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Text.RegularExpressions;

namespace MvcApplication1.Controllers
{ 
    public class TempRoutineController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();

        
        //
        //Get Empty Slots
        //[Authorize (Roles="Teacher")]
        [Authorize]
        public ViewResult ShowEmptySlots(string department, string semeType, string year, string day) 
        {


            Response.AddHeader("Refresh", "180");
            var routines = db.Routines.Include(t => t.Course).Include(t => t.Department).Include(t => t.ClassRoom).Include(t => t.Teacher).Include(t => t.Semester).Include(t => t.RoutineType);
            routines = routines.Where(a => a.Course.Code == "---").Where(a => a.Teacher.Initial == "---");



            //to populate Department DropDown List
            var DeptLst = new List<string>();
            var DeptQry = from d in db.Routines
                          orderby d.Department.Code
                          select d.Department.Code;

            DeptLst.AddRange(DeptQry.Distinct());
            ViewBag.department = new SelectList(DeptLst);


            //to populate semestertype DropDown List
            var SemeTypeLst = new List<string>();
            var SemeTypeQry = from d in db.Routines
                              orderby d.Semester.Name
                              select d.Semester.Name;

            SemeTypeLst.AddRange(SemeTypeQry.Distinct());
            ViewBag.semeType = new SelectList(SemeTypeLst);

            //to populate year DropDown List
            var YearLst = new List<string>();
            var YearQry = from d in db.Routines
                          orderby d.Year
                          select d.Year;

            YearLst.AddRange(YearQry.Distinct());
            ViewBag.year = new SelectList(YearLst);

            //to populate Day DropDown List
            var DayLst = new List<string>();
            var DayQry = from d in db.Routines
                         orderby d.Day
                         select d.Day;

            DayLst.AddRange(DayQry.Distinct());
            ViewBag.Day = new SelectList(DayLst);

            if (!string.IsNullOrEmpty(department))
            {
                routines = routines.Where(x => x.Department.Code == department);
            }
            if (!string.IsNullOrEmpty(semeType))
            {
                routines = routines.Where(x => x.Semester.Name == semeType);
            }
            if (!string.IsNullOrEmpty(year))
            {
                routines = routines.Where(x => x.Year == year);
            }
            if (!string.IsNullOrEmpty(day))
            {
                routines = routines.Where(x => x.Day == day);
            }



            if (!string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(semeType) && !string.IsNullOrEmpty(year))
            {
                return View(routines.ToList());
            }
            else 
            {
                ViewBag.ErrorMessage = "Semester , Year  and Department cannot be empty.";
                return View();
            }
            
        }
        
        //
        // GET: /TempRoutine/
        [Authorize]
        public ViewResult Index()
        {
            Response.AddHeader("Refresh", "60");
            List<TempRoutine>aList=new List<TempRoutine>();
            var routines = db.TempRoutines.Include(t => t.Course).Include(t => t.Department).Include(t => t.ClassRoom).Include(t => t.Teacher).Include(t => t.Semester).Include(t => t.RoutineType);

            return View(routines.ToList());
        }

        //
        // GET: /TempRoutine/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            TempRoutine temproutine =db.TempRoutines.Find(id);
            return View(temproutine);
        }

        //
        // GET: /TempRoutine/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Teacher"))
                {
                    TempRoutine aTempRoutine = new TempRoutine();
                    if (id != null)
                    {
                        Routine aRoutine = db.Routines.Where(a => a.RoutineId == id).Select(a => a).FirstOrDefault();
                        aTempRoutine.Day = aRoutine.Day;
                        aTempRoutine.Department = aRoutine.Department;
                        aTempRoutine.RoutineDepartmentId = aRoutine.RoutineDepartmentId;
                        aTempRoutine.Time_From = aRoutine.Time_From;
                        aTempRoutine.Time_To = aRoutine.Time_To;
                        aTempRoutine.ClassRoom = aRoutine.ClassRoom;
                        aTempRoutine.RoutineClassroomId = aRoutine.RoutineClassroomId;
                        aTempRoutine.RoutineType = aRoutine.RoutineType;

                        aTempRoutine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Temporary").Select(a=>a.RoutineTypeId).FirstOrDefault();

                        aRoutine.Year = Regex.Replace(aRoutine.Year, @"\0", "");
                        aTempRoutine.Year = aRoutine.Year;
                        aTempRoutine.Semester = aRoutine.Semester;
                        aTempRoutine.RoutineSemesterId = aRoutine.RoutineSemesterId;

                        aTempRoutine.UserName = User.Identity.Name;
                        aTempRoutine.RoutineTeacherId = db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Teacher.TeacherId).FirstOrDefault();
                        aTempRoutine.RoutineDepartmentId = db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Teacher.TeacherDeptId).FirstOrDefault();
                        aTempRoutine.TempRoutineDate = System.DateTime.Now;
                        if (aTempRoutine.RoutineTeacherId == 0 || aTempRoutine.RoutineDepartmentId == 0)
                        {
                            ViewBag.ErrorMsg = "You Do not have any teacher initial in the routine.So you can not assign a make up class.";
                            ViewBag.message = "You Do not have any teacher initial in the routine.So you can not assign a make up class.";
                            return View("Error");
                        }

                    }
                    else
                    {
                        aTempRoutine = null;
                        ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                        ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName");
                        ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
                    }

                    ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code");
                    ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo");
                   
                    return View(aTempRoutine);
                }
                ViewBag.message = "Please Contact with the Admin because you are not authorize to do this.";
                return View("Error","Upload");
            }
            catch
            {
                ViewBag.M = "Unable to load Create TempRoutineController";
                return View();
            }
        } 

        //
        // POST: /TempRoutine/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(TempRoutine temproutine)
        {
            try
            {
                //temproutine.UserName = User.Identity.Name;

                if (ModelState.IsValid)
                {
                    if (checkPossibilitywithDate(temproutine) == true)
                    {
                        if (CheckPossibilityWithUser(temproutine) == true)
                        {
                            if (CheckPossibilityWithUser2(temproutine) == true)
                            {
                                if (checkPossibilitywithDay(temproutine) == true)
                                {
                                    if (CheckPossibilityWithTimeSlot(temproutine) == true)
                                    {
                                        temproutine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Temporary").Select(a => a.RoutineTypeId).FirstOrDefault();
                                        db.TempRoutines.Add(temproutine);
                                        db.SaveChanges();
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        string ft = temproutine.Time_From.TimeOfDay.ToString();
                                        string tt = temproutine.Time_To.TimeOfDay.ToString();
                                        string room = db.ClassRooms.Where(a => a.ClassRoomId == temproutine.RoutineClassroomId).Select(a => a.RoomNo).FirstOrDefault();
                                        string givendate = temproutine.TempRoutineDate.Date.ToShortDateString();
                                        string chosenDay = temproutine.TempRoutineDate.DayOfWeek.ToString();
                                        string msg = "Room " + room + " is not available at " + givendate + " ( " + chosenDay + " )" + " from " + ft + " to " + tt;
                                        ViewBag.message = msg;
                                        ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code");
                                        ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                                        ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                                        ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                                        ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                                        ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                                        ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                                        return View(temproutine);
                                    }
                                }
                                else
                                {
                                    var name = User.Identity.Name;
                                    DateTime day = temproutine.TempRoutineDate;
                                    DateTime FromDate = GetFromDate(day);
                                    DateTime ToDate = GetToDate(day);
                                    string msg = "You have already Booked 3 or more classes from ''" + FromDate.GetDateTimeFormats()[3].ToString() + "''  to  ''" + ToDate.GetDateTimeFormats()[3].ToString()+"''";
                                    ViewBag.message = msg;
                                    ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code"); 
                                    ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                                    ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                                    ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                                    ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                                    ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                                    ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                                    return View(temproutine);
                                }
                            }
                            else
                            {
                                string chosenDay = temproutine.TempRoutineDate.DayOfWeek.ToString();
                                string msg = "The Day you choose is " + chosenDay + " which Does not match with make up slot day " + temproutine.Day;
                                ViewBag.message = msg;
                                ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code"); 
                                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                                return View(temproutine);
                            }
                        }
                        else
                        {
                            string msg = "You have already Assigned a Class at " + temproutine.TempRoutineDate.Date.ToShortDateString();
                            ViewBag.message = msg;
                            ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code");
                            ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                            ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                            ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                            ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                            ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                            return View(temproutine);
                        }
                    }
                    else
                    {
                        ViewBag.message = "Assigned Date have to be greater than Today";
                        ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code");
                        ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                        ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                        ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                        ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                        ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                        ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                        return View(temproutine);
                    }
                }

                ViewBag.RoutineCourseId = new SelectList(db.Routines.Where(a => a.Teacher.Initial == User.Identity.Name).Select(a => a.Course).Distinct(), "CourseId", "Code");
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", temproutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", temproutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                return View(temproutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Create TempRoutineController";
                return View();
            }
        }

        private bool CheckPossibilityWithUser2(TempRoutine temproutine)
        {
            var name = User.Identity.Name;
            DateTime day = temproutine.TempRoutineDate;
            DateTime FromDate = GetFromDate(day);
            DateTime ToDate = GetToDate(day);
            int countchk = db.TempRoutines.Where(a => a.UserName == name).Where(a => a.TempRoutineDate >= FromDate && a.TempRoutineDate <= ToDate).Count();
            if (countchk < 3) 
            {
                return true;
            }
            return false;
        }

        private bool checkPossibilitywithDay(TempRoutine temproutine)
        {
            string chosenDay = temproutine.TempRoutineDate.DayOfWeek.ToString();
            if (chosenDay == temproutine.Day) 
            {
                return true;
            }

            return false;
        }

        private bool CheckPossibilityWithTimeSlot(TempRoutine temproutine)
        {
            List<TempRoutine> routineList = db.TempRoutines.Where(a => a.TempRoutineDate == temproutine.TempRoutineDate).Where(a => a.RoutineClassroomId == temproutine.RoutineClassroomId).Where(a=>a.Time_From==temproutine.Time_From).Select(a => a).ToList();
            if (routineList.Count > 0) 
            {
                return false;
            }
            //return true;
            return true;
        }

        private bool CheckPossibilityWithUser(TempRoutine temproutine)
        {
            var name = User.Identity.Name;
            
            //
            //To test with today
            //DateTime day = DateTime.Now;

            //To test with given date
            //DateTime day = temproutine.TempRoutineDate;

            //try
            //{
            //    DateTime FromDate =GetFromDate(day);
            //    DateTime ToDate = GetToDate(day);

            //    int countchk = db.TempRoutines.Where(a => a.UserName == name).Where(a => a.TempRoutineDate >= FromDate && a.TempRoutineDate <= ToDate).Count();
            //}
            //catch { }
            int count = db.TempRoutines.Where(a => a.UserName == name).Where(a=>a.TempRoutineDate==temproutine.TempRoutineDate).Count();
            if (count > 0) 
            {
                return false;
            }
            return true;
        }

        private DateTime GetToDate(DateTime day)
        {
            int year = day.Year;
            int month = day.Month;
            int today = day.Day;

            int newmonth = month;
            int newday = today+3;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                if (today >28)
                {
                    newmonth = month + 1;
                    newday = (today + 3)%31;
                }
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                if (today >27)
                {
                    newmonth = month +1;
                    newday = (today + 3) % 30;
                }
            }
            else
            {
                if (today > 25 && month==2)
                {
                    newmonth = month + 1;
                    newday = (today + 3) % 28;
                }
            }
            DateTime aDate = new DateTime(year, newmonth, newday);
            return aDate;

        }

        private DateTime GetFromDate(DateTime day)
        {
            int year = day.Year;
            int month = day.Month;
            int today = day.Day;

            int newmonth=month;
            int newday=today-3;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) 
            {
                if (today < 4) 
                {
                    newmonth = month - 1;
                    if(month==3)
                    {
                        newday=today+28-3;
                    }
                    else
                    {
                        newday = today + 30 - 3;
                    }
                }
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                if (today < 3)
                {
                    newmonth = month - 1;
                    newday = today + 31 - 3;
                }
            }
            else
            {
                if (today <3 && month == 2)
                {
                    newmonth = month - 1;
                    newday = today + 31 - 3;
                }
            }

            DateTime aDate=new DateTime(year,newmonth,newday);
            return aDate;
        }

        private bool checkPossibilitywithDate(TempRoutine temproutine)
        {
            //temproutine.TempRoutineDate.TimeOfDay = temproutine.Time_From.TimeOfDay;
            var currentDay = DateTime.Now;
            //var CurrentTime=DateTime.Now.TimeOfDay;
            if (DateTime.Compare(temproutine.TempRoutineDate, currentDay) == 1) 
            {    
                return true;
            }
            //var testDay = DateTime.Now.ToString();
            var testDay1 = DateTime.Now.Date.ToShortDateString();
            //var test1 = DateTime.Compare(testDay, temproutine.TempRoutineDate);
            //DateTime aDate=Convert.ToDateTime("11/23/2010 09:05:00 PM");
            //var test11 = DateTime.Compare(testDay,aDate );
            //var test12 = DateTime.Compare(aDate, aDate);
            //var test2 = DateTime.Today;
            //var test13 = DateTime.Now.TimeOfDay;
            //var test14 = DateTime.Now.TimeOfDay.CompareTo(test13);
            //var test15 = DateTime.Now.TimeOfDay.CompareTo(aDate.TimeOfDay);
            //var test3 = DateTime.Now.Day.ToString();
            //var test4 = DateTime.Now.DayOfWeek.ToString();
            //return true;
            return false; 
        }
        
        //
        // GET: /TempRoutine/Edit/5
 [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                TempRoutine temproutine = db.TempRoutines.Find(id);
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", temproutine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", temproutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", temproutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                return View(temproutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit TempRoutineController";
                return View();
            }
        }

        //
        // POST: /TempRoutine/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(TempRoutine temproutine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.Name == temproutine.UserName)
                    {
                        db.Entry(temproutine).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        ViewBag.message = "You are not the creator of this Routine So You can not Edit It";
                        ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", temproutine.RoutineCourseId);
                        ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", temproutine.RoutineDepartmentId);
                        ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                        ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", temproutine.RoutineTeacherId);
                        ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                        ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                        return View(temproutine);
                    }
                }
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", temproutine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", temproutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", temproutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", temproutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", temproutine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", temproutine.RoutineTypeId);
                return View(temproutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit TempRoutineController";
                return View();
            }
        }

        //
        // GET: /TempRoutine/Delete/5
 [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                TempRoutine temproutine = db.TempRoutines.Find(id);
                return View(temproutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Delete TempRoutineController";
                return View();
            }
        }

        //
        // POST: /TempRoutine/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TempRoutine temproutine = db.TempRoutines.Find(id);
                if (User.Identity.Name == temproutine.UserName)
                {
                    BackupHelper aBackupHelper = new BackupHelper();
                    db.TempRoutineBackupModels.Add(aBackupHelper.GetTempRoputineBackupData(temproutine));
                    db.Routines.Remove(temproutine);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else 
                {
                    ViewBag.message = "You are not the creator of this Routine So You can not Delete It";
                    return View("Delete",temproutine);
                }
            }
            catch
            {
                ViewBag.M1 = "Unable to load DeleteConfirmed TempRoutineController";
                return View("Delete");
            }
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}