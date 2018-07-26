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
    public class UploadCourseController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();
        //
        // GET: /UploadCouse/
         [Authorize(Roles = "Admin")]

        public ActionResult Index(string Body)
        {
            try
            {
                string textAreaData = Body;
                string[] delimeter = { Environment.NewLine };
                if (textAreaData != null)
                {
                    string[] aString = textAreaData.Split(delimeter, StringSplitOptions.None);
                    int q = aString.Length;
                    for (int i = 0; i < q; i++)
                    {
                        string[] delimeter1 = new string[1];
                        delimeter1[0] = "\t";
                        string[] word = aString[i].Split(delimeter1, StringSplitOptions.None);
                        int k = word.Length;

                        if (k == 6)
                        {
                            Course aCourse = new Course();
                            aCourse.Name = word[0];
                            aCourse.Code = word[1];
                            aCourse.Credit = word[2];
                            aCourse.Level = word[3];
                            aCourse.Term = word[4];
                            string deptCode = word[5];
                            aCourse.CourseDeptId = db.Departments.Where(a => a.Code == deptCode).Select(a => a.DepartmentId).FirstOrDefault();
                            if (db.Courses.Where(a => a.Code == aCourse.Code).Count() < 1)
                            {
                                db.Courses.Add(aCourse);
                                db.SaveChanges();
                            }
                            //ViewBag.Message ="Data Successfully Updated";
                        }

                    }

                    return RedirectToAction("Index", "Course");
                }
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Index UploadCourseController";
                return View();
            }
            
        }

    }
}
