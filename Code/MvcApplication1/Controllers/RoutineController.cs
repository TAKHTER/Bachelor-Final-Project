using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Printing;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{ 
    public class RoutineController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();


        //
        // GET: /Routine/
       // [Authorize(Roles = "Admin")]
        public ViewResult Index(string teacherInitial, string roomNo, string courseCode, string Day, string timeFrom, string section, string department, string routineType, string semeType, string year) //DateTime TimeFrom
        {
            //Response.AddHeader("Refresh","5");
            //var routines = db.Routines.Include(r => r.Course).Include(r => r.ClassRoom).Include(r => r.Teacher).Include(r => r.Semester);
            var routines = db.Routines.Include(r => r.Course).Include(r => r.Department).Include(r => r.ClassRoom).Include(r => r.Teacher).Include(r => r.Semester).Include(r=>r.RoutineType).Include(r=>r.Semester);


            //to populate Department DropDown List
            var DeptLst = new List<string>();
            var DeptQry = from d in db.Routines
                         orderby d.Department.Code
                         select d.Department.Code;

            DeptLst.AddRange(DeptQry.Distinct());
            ViewBag.department = new SelectList(DeptLst);

            //to populate routinetype DropDown List
            var RoutineTypeLst = new List<string>();
            var RoutineTypeQry = from d in db.Routines
                          orderby d.RoutineType.TypeName
                          select d.RoutineType.TypeName;

            RoutineTypeLst.AddRange(RoutineTypeQry.Distinct());
            ViewBag.routineType = new SelectList(RoutineTypeLst);

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

            //to populate Section DropDown List
            var SecLst = new List<string>();
            var SecQry = from d in db.Routines orderby d.Section select d.Section;
            SecLst.AddRange(SecQry.Distinct());
            ViewBag.section = new SelectList(SecLst);

            //to populate TimeFrom DropDown List
            var TimeFromLst = new List<string>();
            var TimeFromQry = from d in db.Routines
                              orderby d.Time_From
                              select d.Time_From;
            //TimeFromLst.AddRange(TimeFromQry.Distinct());
            //ViewBag.TimeFrom = new SelectList(TimeFromLst);

            //to populate Initial DropDown List
            var TeacherLst = new List<string>();
            var TeacherQry = from d in db.Routines
                             orderby d.Teacher.Initial
                             select d.Teacher.Initial;
            TeacherLst.AddRange(TeacherQry.Distinct());
            ViewBag.teacherInitial = new SelectList(TeacherLst);

            //to populate RoomNo DropDown List
            var RoomList = new List<string>();
            var RoomQry = from d in db.Routines
                          orderby d.ClassRoom.RoomNo
                          select d.ClassRoom.RoomNo;
            RoomList.AddRange(RoomQry.Distinct());
            ViewBag.roomNo = new SelectList(RoomList);

            //to populate coursecode DropDown List
            var courseCodeLst = new List<string>();
            var courseCodeQry = from d in db.Routines
                                orderby d.Course.Code
                                select d.Course.Code;
            courseCodeLst.AddRange(courseCodeQry.Distinct());
            ViewBag.courseCode = new SelectList(courseCodeLst);


            //var finalroutines = routines;
            if (!string.IsNullOrEmpty(department))
            {
                routines = routines.Where(x => x.Department.Code == department);
            }
            if (!string.IsNullOrEmpty(routineType))
            {
                routines = routines.Where(x => x.RoutineType.TypeName == routineType);
            }
            if (!string.IsNullOrEmpty(semeType))
            {
                routines = routines.Where(x => x.Semester.Name == semeType);
            }
            if (!string.IsNullOrEmpty(year))
            {
                routines = routines.Where(x => x.Year == year);
            }
            if (!string.IsNullOrEmpty(teacherInitial))
            {
                routines = routines.Where(x => x.Teacher.Initial == teacherInitial);
            }

            if (!string.IsNullOrEmpty(roomNo))
            {
                routines = routines.Where(x => x.ClassRoom.RoomNo == roomNo);
            }

            //if (!string.IsNullOrEmpty(TimeFrom))
            //{
            //    routines = routines.Where(x => x.Time_From == TimeFrom);
            //}

            if (!string.IsNullOrEmpty(section))
            {
                routines = routines.Where(x => x.Section == section);
            }

            if (!string.IsNullOrEmpty(Day))
            {
                routines = routines.Where(x => x.Day == Day);
            }

            if (!string.IsNullOrEmpty(courseCode))
            {
                routines = routines.Where(x => x.Course.Code == courseCode);
            }
            int itemCount = routines.Count();
            if (itemCount == 0)
            {
                ViewBag.Message = "No data found";
            }
            return View(routines.ToList());
      
        }

        //
        // GET: /Routine/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            Routine routine = db.Routines.Find(id);
            return View(routine);
        }

        //
        // GET: /Routine/Create
       [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Code");
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo");
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial");
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                //ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName");
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Create RoutineController";
                return View();
            }
        } 

        //
        // POST: /Routine/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Routine routine)
        {
            try
            {
                routine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Day").Select(a => a.RoutineTypeId).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    db.Routines.Add(routine);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", routine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", routine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", routine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", routine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", routine.RoutineSemesterId);
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                //ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", routine.RoutineTypeId);
                return View(routine);
            }
            catch
            {
                ViewBag.M = "Unable to load Create RoutineController";
                return View();
            }
        }
        
        //
        // GET: /Routine/Edit/5
[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                Routine routine = db.Routines.Find(id);
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Code", routine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", routine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", routine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", routine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", routine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", routine.RoutineTypeId);
                return View(routine);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit RoutineController";
                return View();
            }
        }

        //
        // POST: /Routine/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Routine routine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(routine).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", routine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", routine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", routine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", routine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", routine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", routine.RoutineTypeId);
                return View(routine);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit RoutineController";
                return View();
            }
        }

        //
        // GET: /Routine/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                Routine routine = db.Routines.Find(id);
                return View(routine);
            }
            catch
            {
                ViewBag.M = "Unable to load Delete RoutineController";
                return View();
            }
        }

        //
        // POST: /Routine/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Routine routine = db.Routines.Find(id);

                BackupHelper aBackupHelper = new BackupHelper();
                string username=User.Identity.Name;
                db.RoutineBackupModels.Add(aBackupHelper.GetRoutineBackup(routine, username));
                db.Routines.Remove(routine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.M1 = "Unable to load DeleteConfirmed RoutineController";
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