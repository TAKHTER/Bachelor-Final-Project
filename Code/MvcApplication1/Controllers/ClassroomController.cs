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
    public class ClassroomController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();

        //
        // GET: /Classroom/
    [Authorize(Roles = "Admin")]
        public ViewResult Index(string roomNo )
        {
            try
            {
                var classRooms = db.ClassRooms.ToList();

                //to populate RoomNo DropDown List
                var RoomList = new List<string>();
                var RoomQry = from d in db.ClassRooms
                              orderby d.RoomNo
                              select d.RoomNo;
                RoomList.AddRange(RoomQry.Distinct());
                ViewBag.roomNo = new SelectList(RoomList);

                if (!string.IsNullOrEmpty(roomNo))
                {
                    classRooms = classRooms.Where(x => x.RoomNo == roomNo).ToList();
                }
                return View(classRooms);
            }
            catch
            {
                ViewBag.M = "Unable to load Index ClassroomController";
                return View();
            }
        }

        //
        // GET: /Classroom/Details/5
         [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            ClassRoom classroom = db.ClassRooms.Find(id);
            return View(classroom);
        }

        //
        // GET: /Classroom/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Create ClassroomController";
                return View();
            }
        } 

        //
        // POST: /Classroom/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(ClassRoom classroom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ClassRooms.Add(classroom);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(classroom);
            }
            catch
            {
                ViewBag.M = "Unable to load Create ClassroomController";
                return View();
            }
        }
        
        //
        // GET: /Classroom/Edit/5
  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                ClassRoom classroom = db.ClassRooms.Find(id);
                return View(classroom);
            }
            catch
            {
                ViewBag.M = "Unable to load eDIT ClassroomController";
                return View();
            }
        }

        //
        // POST: /Classroom/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(ClassRoom classroom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(classroom).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(classroom);
            }
            catch
            {
                ViewBag.M = "Unable to load Edit ClassroomController";
                return View();
            }
        }

        //
        // GET: /Classroom/Delete/5
  //[Authorize(Roles = "Admin")]
  //      public ActionResult Delete(int id)

  //      {
  //          try
  //          {
  //              ClassRoom classroom = db.ClassRooms.Find(id);
  //              return View(classroom);
  //          }
  //          catch
  //          {
  //              ViewBag.M = "Unable to load Delete ClassroomController";
  //              return View();
  //          }
  //      }

  //      //
  //      // POST: /Classroom/Delete/5
  //       [Authorize(Roles = "Admin")]
  //      [HttpPost, ActionName("Delete")]
  //      public ActionResult DeleteConfirmed(int id)
  //      {
  //          try
  //          {
  //              ClassRoom classroom = db.ClassRooms.Find(id);
  //              db.ClassRooms.Remove(classroom);
  //              db.SaveChanges();
  //              return RedirectToAction("Index");
  //          }
  //          catch
  //          {
  //              ViewBag.M1 = "Unable to load DeleteConfirmed ClassroomController";
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
