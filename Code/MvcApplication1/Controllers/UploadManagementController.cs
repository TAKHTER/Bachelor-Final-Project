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
    public class UploadManagementController : Controller
    {
        private AllEntitiesContext db = new AllEntitiesContext();
        private ParseHelper aParseHelper = new ParseHelper();
        //
        // GET: /UploadManagement/
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

                        if (k == 4)
                        {
                            Teacher aTeacher = new Teacher();
                            aTeacher.Name = word[0];
                            aTeacher.Initial = word[1];
                            aTeacher.Designation = word[2];
                            string deptCode = word[3];
                            aTeacher.TeacherDeptId = db.Departments.Where(a => a.Code == deptCode).Select(a => a.DepartmentId).FirstOrDefault();
                            if (db.Teachers.Where(a => a.Initial == aTeacher.Initial).Count() < 1)
                            {
                                try
                                {
                                    aParseHelper.createIdForTeacher(aTeacher.Initial);
                                }
                                catch { }
                                db.Teachers.Add(aTeacher);
                                db.SaveChanges();
                            }
                            //ViewBag.Message ="Data Successfully Updated";
                        }

                    }

                    return RedirectToAction("Index", "Teacher");
                }
                return View();
                //return RedirectToAction("Index","Teacher");
            }
            catch
            {
               ViewBag.M= "Unable to load Index UploadManagementController";
                return View();
            }
        }

    }
}
