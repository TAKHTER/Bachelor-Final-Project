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
    public class DepartmentController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();

        //
        // GET: /Department/
        [Authorize(Roles = "Admin")]
        public ViewResult Index(string DeptCode)
        {
            var deptcodes = db.Departments.ToList();
            //to populate Deptcode DropDown List
            var DeptCodeLst = new List<string>();
            var DeptCodeQry = from d in db.Departments
                                orderby d.Code
                                select d.Code;
            DeptCodeLst.AddRange(DeptCodeQry.Distinct());
            ViewBag.DeptCode = new SelectList(DeptCodeLst);

            if (!string.IsNullOrEmpty(DeptCode))
            {
                deptcodes = deptcodes.Where(a => a.Code == DeptCode).ToList();
            }



            return View(deptcodes);
        }

        //
        // GET: /Department/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // GET: /Department/Create
         [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Creae DepartmentController";
                return View();
            }
        } 

        //
        // POST: /Department/Create
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(department);
            }
            catch
            {
                ViewBag.M = "Unable to load Create DepartmentController";
                return View();
            }
        }
        
        //
        // GET: /Department/Edit/5
  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)

        {
            try
            {
                Department department = db.Departments.Find(id);
                return View(department);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit DepartmentController";
                return View();
            }
        }

        //
        // POST: /Department/Edit/5
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(department);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit DepartmentController";
                return View();
            }
        }

        //
        // GET: /Department/Delete/5
  //[Authorize(Roles = "Admin")]
  //      public ActionResult Delete(int id)
  //      {
  //          try
  //          {
  //              Department department = db.Departments.Find(id);
  //              return View(department);
  //          }
  //          catch
  //          {
  //              ViewBag.M = "Unable to load Delete DepartmentController";
  //              return View();
  //          }
  //      }

  //      //
  //      // POST: /Department/Delete/5
  //      [Authorize(Roles = "Admin")]
  //      [HttpPost, ActionName("Delete")]
  //      public ActionResult DeleteConfirmed(int id)
  //{
  //    try
  //    {
  //        Department department = db.Departments.Find(id);
  //        db.Departments.Remove(department);
  //        db.SaveChanges();
  //        return RedirectToAction("Index");
  //    }
  //    catch
  //    {
  //        ViewBag.M1 = "Unable to load DeleteConfirmed DepartmentController";
  //        return View("Delete");
  //    }
  //      }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}