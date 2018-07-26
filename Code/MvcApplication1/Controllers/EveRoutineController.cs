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
    public class EveRoutineController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();

        //
        // GET: /EveRoutine/
        // [Authorize(Roles = "Admin")]
        public ViewResult Index(string teacherInitial, string roomNo, string courseCode, string Day, string section, string department, string semeType, string year)
        {
            var routines = db.EveRoutines.Include(e => e.Course).Include(e => e.Department).Include(e => e.ClassRoom).Include(e => e.Teacher).Include(e => e.Semester).Include(e => e.RoutineType);


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

            //to populate Section DropDown List
            var SecLst = new List<string>();
            var SecQry = from d in db.Routines orderby d.Section select d.Section;
            SecLst.AddRange(SecQry.Distinct());
            ViewBag.section = new SelectList(SecLst);

            //to populate TimeFrom DropDown List
            //var TimeFromLst = new List<string>();
            //var TimeFromQry = from d in db.Routines
            //                  orderby d.Time_From
            //                  select d.Time_From;
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
        // GET: /EveRoutine/Details/5
         [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            EveRoutine everoutine = db.EveRoutines.Find(id);
            return View(everoutine);
        }

        //
        // GET: /EveRoutine/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()

        {
            try
            {
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Code");
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo");
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial");
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
                //ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName");
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Create EveRoutineController";
                return View();
            }
        } 

        //
        // POST: /EveRoutine/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(EveRoutine everoutine)
        {
            try
            {
                everoutine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Evening").Select(a => a.RoutineTypeId).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    db.EveRoutines.Add(everoutine);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", everoutine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", everoutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", everoutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", everoutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", everoutine.RoutineSemesterId);
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                //ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", everoutine.RoutineTypeId);
                return View(everoutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Create EveRoutineController";
                return View();
            }
        }
        
        //
        // GET: /EveRoutine/Edit/5
  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                EveRoutine everoutine = db.EveRoutines.Find(id);
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Code", everoutine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", everoutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", everoutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Initial", everoutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", everoutine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", everoutine.RoutineTypeId);
                return View(everoutine);
            }
            catch
            {
               ViewBag.M="Unable to load Edit EveRoutineController";
                return View();
            }
        }

        //
        // POST: /EveRoutine/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EveRoutine everoutine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(everoutine).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RoutineCourseId = new SelectList(db.Courses, "CourseId", "Name", everoutine.RoutineCourseId);
                ViewBag.RoutineDepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", everoutine.RoutineDepartmentId);
                ViewBag.RoutineClassroomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", everoutine.RoutineClassroomId);
                ViewBag.RoutineTeacherId = new SelectList(db.Teachers, "TeacherId", "Name", everoutine.RoutineTeacherId);
                ViewBag.RoutineSemesterId = new SelectList(db.Semesters, "SemesterId", "Name", everoutine.RoutineSemesterId);
                ViewBag.RoutineTypeId = new SelectList(db.RoutineTypes, "RoutineTypeId", "TypeName", everoutine.RoutineTypeId);
                return View(everoutine);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit EveRoutineController";
                return View();
            }
        }

        //
        // GET: /EveRoutine/Delete/5
  [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                EveRoutine everoutine = db.EveRoutines.Find(id);
                return View(everoutine);
            }
            catch
            {
               ViewBag.M= "Unable to load Delete EveRoutineController";
                return View();
            }
        }

        //
        // POST: /EveRoutine/Delete/5
         [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                EveRoutine everoutine = db.EveRoutines.Find(id);
                
                BackupHelper aBackupHelper = new BackupHelper();
                string username=User.Identity.Name;
                db.EveRoutineBackupModels.Add(aBackupHelper.GetEveRoutineBackup(everoutine,username));

                db.Routines.Remove(everoutine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.M1 = "Unable to load DeleteConfirmed EveRoutineController";
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