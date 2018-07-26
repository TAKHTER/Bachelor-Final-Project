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
    public class CourseController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();

        //
        // GET: /Course/
        [Authorize(Roles = "Admin")]
        public ViewResult Index(string courseCode)
        {
            var courses = db.Courses.Include(c => c.Department);

            //to populate coursecode DropDown List
            var courseCodeLst = new List<string>();
            var courseCodeQry = from d in db.Courses
                                orderby d.Code
                                select d.Code;
            courseCodeLst.AddRange(courseCodeQry.Distinct());
            ViewBag.courseCode = new SelectList(courseCodeLst);

            if (!string.IsNullOrEmpty(courseCode))
            {
                courses = courses.Where(x => x.Code == courseCode);
            }

            return View(courses.ToList());
        }

        //
        // GET: /Course/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        //
        // GET: /Course/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.CourseDeptId = new SelectList(db.Departments, "DepartmentId", "Code");
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Create CourseController";
                return View();
            }
        } 

        //
        // POST: /Course/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CourseDeptId = new SelectList(db.Departments, "DepartmentId", "Name", course.CourseDeptId);
                return View(course);
            }
            catch
            {
                ViewBag.M = "Unable to load Create CourseController";
                return View();
            }
        }
        
        //
        // GET: /Course/Edit/5
  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                Course course = db.Courses.Find(id);
                ViewBag.CourseDeptId = new SelectList(db.Departments, "DepartmentId", "Name", course.CourseDeptId);
                return View(course);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit CourseController";
                return View();
            }
        }

        //
        // POST: /Course/Edit/5
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CourseDeptId = new SelectList(db.Departments, "DepartmentId", "Name", course.CourseDeptId);
                return View(course);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit CourseController";
                return View();
            }
        }

        //
        // GET: /Course/Delete/5
  //[Authorize(Roles = "Admin")]
  //      public ActionResult Delete(int id)
  //      {
  //          try
  //          {
  //              Course course = db.Courses.Find(id);
  //              return View(course);
  //          }
  //          catch
  //          {
  //              ViewBag.M1 = "Unable to load Delete CourseController";
  //              return View();
  //          }
  //      }

  //      //
  //      // POST: /Course/Delete/5
  //      [Authorize(Roles = "Admin")]
  //      [HttpPost, ActionName("Delete")]
  //      public ActionResult DeleteConfirmed(int id)
  //      {
  //          try
  //          {
  //              Course course = db.Courses.Find(id);
  //              db.Courses.Remove(course);
  //              db.SaveChanges();
  //              return RedirectToAction("Index");
  //          }
  //          catch
  //          {
  //              ViewBag.M="Unable to load DeleteConfirmed CourseController";
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