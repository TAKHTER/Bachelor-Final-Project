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
    public class TeacherController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();
        private ParseHelper aParseHelper = new ParseHelper();
        //
        // GET: /Teacher/
         [Authorize(Roles = "Admin")]
        public ViewResult Index(string teacherInitial)
        {
            var teachers = db.Teachers.Include(a=>a.Department);

            //to populate Initial DropDown List
            var TeacherLst = new List<string>();
            var TeacherQry = from d in db.Teachers
                             orderby d.Initial
                             select d.Initial;
            TeacherLst.AddRange(TeacherQry.Distinct());
            ViewBag.teacherInitial = new SelectList(TeacherLst);

            if (!string.IsNullOrEmpty(teacherInitial))
            {
                teachers = teachers.Where(t => t.Initial == teacherInitial);
            }

            return View(teachers.ToList());
        }

        //
        // GET: /Teacher/Details/5
         [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            return View(teacher);
        }

        //
        // GET: /Teacher/Create
         [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.TeacherDeptId = new SelectList(db.Departments, "DepartmentId", "Code");
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Create TeacherController";
                return View();
            }
        } 

        //
        // POST: /Teacher/Create
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int tcount = db.Teachers.Where(a => a.Initial == teacher.Initial).Count();
                    if (tcount == 0)
                    {
                        try
                        {
                            aParseHelper.createIdForTeacher(teacher.Initial);
                        }
                        catch { }
                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.M = "This Teacher Initial already exists";
                        ViewBag.TeacherDeptId = new SelectList(db.Departments, "DepartmentId", "Name", teacher.TeacherDeptId);
                        return View(teacher);
                    }
                }

                ViewBag.TeacherDeptId = new SelectList(db.Departments, "DepartmentId", "Name", teacher.TeacherDeptId);
                return View(teacher);
            }
            catch
            {
                ViewBag.M = "Unable to load Create TeacherController";
                return View();
            }
        }
        
        //
        // GET: /Teacher/Edit/5
  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                Teacher teacher = db.Teachers.Find(id);
                ViewBag.TeacherDeptId = new SelectList(db.Departments, "DepartmentId", "Name", teacher.TeacherDeptId);
                return View(teacher);
            }
            catch
            {
               ViewBag.M= "Unable to load Edit TeacherController";
                return View();
            }
        }

        //
        // POST: /Teacher/Edit/5
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.TeacherDeptId = new SelectList(db.Departments, "DepartmentId", "Name", teacher.TeacherDeptId);
                return View(teacher);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit TeacherController";
                return View();
            }
        }

        //
        // GET: /Teacher/Delete/5
  //[Authorize(Roles = "Admin")]
  //      public ActionResult Delete(int id)
  //      {
  //          try
  //          {
  //              Teacher teacher = db.Teachers.Find(id);
  //              return View(teacher);
  //          }
  //          catch
  //          {
  //              ViewBag.M = "Unable to load Delete TeacherController";
  //              return View();
  //          }
  //      }

  //      //
  //      // POST: /Teacher/Delete/5
  //      [Authorize(Roles = "Admin")]
  //      [HttpPost, ActionName("Delete")]
  //      public ActionResult DeleteConfirmed(int id)
  //      {
  //          try
  //          {
  //              Teacher teacher = db.Teachers.Find(id);
  //              db.Teachers.Remove(teacher);
  //              db.SaveChanges();
  //              return RedirectToAction("Index");
  //          }
  //          catch
  //          {
  //              ViewBag.M1 = "Unable to load DeleteConfirmed TeacherController";
  //              return View("Delete");
  //          }

  //      }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}