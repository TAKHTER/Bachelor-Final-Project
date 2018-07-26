using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication1.Models
{
    public class CustomLoginController : Controller
    {
        //
        // GET: /CustomLogin/
        private AllEntitiesContext db = new AllEntitiesContext();

        public ActionResult CustomLogOn()
        {
            try
            {
                List<CustomRole> aList = new List<CustomRole>();
                CustomRole aCus = new CustomRole();
                aCus.Name = "Admin";
                aList.Add(aCus);
                CustomRole bCus = new CustomRole();
                bCus.Name = "Teacher";
                aList.Add(bCus);
                ViewBag.Rolename = new SelectList(aList, "Name", "Name");
                ViewBag.SemesterName=new SelectList(db.Semesters,"Name","Name");
                var YearLst = new List<string>();
                var YearQry = from d in db.Routines
                              orderby d.Year
                              select d.Year;

                YearLst.AddRange(YearQry.Distinct());
                ViewBag.year = new SelectList(YearLst);


                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load logno AccountController";
                return View();
            }
        }

        [HttpPost]
        public ActionResult CustomLogOn(UserLogin model, string returnUrl)
        {
            try
            {

                var YearLst=new List<string>();
                if (ModelState.IsValid)
                {
                    if (model.UserRole == String.Empty || model.UserRole == null)
                    {
                        List<CustomRole> bList = new List<CustomRole>();
                        CustomRole cCus = new CustomRole();
                        cCus.Name = "Admin";
                        bList.Add(cCus);
                        CustomRole dCus = new CustomRole();
                        dCus.Name = "Teacher";
                        bList.Add(dCus);
                        ViewBag.Rolename = new SelectList(bList, "Name", "Name");
                        ViewBag.SemesterName = new SelectList(db.Semesters, "Name", "Name");
                         YearLst = new List<string>();
                         var YearQry1 = from d in db.Routines
                                      orderby d.Year
                                      select d.Year;

                        YearLst.AddRange(YearQry1.Distinct());
                        ViewBag.year = new SelectList(YearLst);
                        ViewBag.M1 = "User Role cannot be empty";
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        return View(model);
                    }
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }

                // If we got this far, something failed, redisplay form

                List<CustomRole> aList = new List<CustomRole>();
                CustomRole aCus = new CustomRole();
                aCus.Name = "Admin";
                aList.Add(aCus);
                CustomRole bCus = new CustomRole();
                bCus.Name = "Teacher";
                aList.Add(bCus);
                ViewBag.Rolename = new SelectList(aList, "Name", "Name");
                ViewBag.SemesterName = new SelectList(db.Semesters, "Name", "Name");
                YearLst = new List<string>();
                var YearQry2 = from d in db.Routines
                              orderby d.Year
                              select d.Year;

                YearLst.AddRange(YearQry2.Distinct());
                ViewBag.year = new SelectList(YearLst);
                return View(model);
            }
            catch
            {
                ViewBag.T = "Unable to load LogOn AccountController";
                return View();
            }
        }

    }
}
