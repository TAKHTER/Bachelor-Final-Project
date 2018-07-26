using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class AccountController : Controller
    {
        [Authorize(Roles="Admin")]
        public ActionResult Start()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load start Acco AccountController";
                return View();
            }
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            try
            {
                return View();
            }
            catch 
            {
                ViewBag.M = "Unable to load logno AccountController";
                return View();
            }
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
                return View(model);
            }
            catch
            {
                ViewBag.T = "Unable to load LogOn AccountController";
                return View();
            }
        }


         [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            try
            {
                var roles = Roles.GetAllRoles();
                return View(roles);
            }
            catch
            {
                ViewBag.M = "Unable to load roleindex AccountController";
                return View();
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult RoleCreate()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load rolecreate AccountController";
                return View();
            }
        }

     
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(string RoleName)
        {
            try
            {

                Roles.CreateRole(Request.Form["RoleName"]);
                // ViewBag.ResultMessage = "Role created successfully !";

                return RedirectToAction("RoleIndex", "Account");
            }
            catch
            {
                ViewBag.M = "Unable to load rolecreate AccountController";
                return View();
            }
        }
        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            try
            {
                FormsAuthentication.SignOut();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ViewBag.M1 = "Unable to load logoff AccountController";
                return View("Error", "Upload");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleDelete(string RoleName)
        {
            try
            {
                Roles.DeleteRole(RoleName);
                // ViewBag.ResultMessage = "Role deleted succesfully !";


                return RedirectToAction("RoleIndex", "Account");
            }
            catch
            {
                ViewBag.M = "Unable to load roledelete AccountController";
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            try
            {
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;

                return View();
            }
            catch
            {
                ViewBag.M = " Unable to load RoleAddToUser AccountController";
                return View();
            }
        }

        //
        // Add role to the user
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string RoleName, string UserName)
        {
            try
            {
                if (Roles.IsUserInRole(UserName, RoleName))
                {
                    ViewBag.ResultMessage = "This user already has the role specified !";
                }
                else
                {
                    Roles.AddUserToRole(UserName, RoleName);
                    ViewBag.ResultMessage = "Username added to the role succesfully !";
                }

                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load RoleAddToUser AccountController";
                return View();
            }
        }

        //
        // Get all the roles for a particular user
      
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                    SelectList list = new SelectList(Roles.GetAllRoles());
                    ViewBag.Roles = list;
                }
                return View("RoleAddToUser");
            }
            catch
            {
                ViewBag.M2 = "Unable to load GetRoles AccountController";
                return View("Error","Upload");
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoleForUser()
        {
            try
            {
                ViewBag.RolesForThisUser = Roles.GetRolesForUser();
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load DeleteRoleForUser AccountController";
                return View();
            }
        }


        [HttpPost]
       [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            try
            {
                if (Roles.IsUserInRole(UserName, RoleName))
                {
                    if (RoleName == "Admin")
                    {
                        ViewBag.ResultMessage = "Admin Can not be Deleted !";
                        
                    }
                    else
                    {
                        Roles.RemoveUserFromRole(UserName, RoleName);
                        ViewBag.ResultMessage = "Role removed from this user successfully !";
                        //return View();
                    }
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                    //return View();
                }
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;


                //return View("RoleAddToUser");
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load DeleteRolesForUser AccountController";

                return View();
            }
        }

        //
        // GET: /Account/Register

        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Regist AccountController";
                return View();
            }
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
                    
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        ViewBag.message = "User was Created Successfully";
                        return View();
                        //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        
                        //return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch
            {
                ViewBag.M = "Unable to load Register AccountController";
                return View();
            }
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load ChangePassword AccountController";
                return View();
            }
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // ChangePassword will throw an exception rather
                    // than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch
            {
                ViewBag.M = "Unable to load ChangePassword AccountController";
                return View();
            }
        }


        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load ChangePasswordSuccess AccountController";
                return View();
            }
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
