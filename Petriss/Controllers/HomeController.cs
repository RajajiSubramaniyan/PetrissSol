﻿using Petriss.Models.EntityManager;
using Petriss.Models.ViewModel;
using Petriss.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Petriss.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult Welcome()
        {
            return View();
        }


        public ActionResult UnAuthorized()
        {
            return View();
        }

        //[AuthorizeRoles("Admin")]
        //[Authorize(Roles = "Member")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        //[AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);

                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";

                ViewBag.Message = message;

                return PartialView(UDV);
            }

            return RedirectToAction("Index", "Home");
        }

        //[AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string emailid, string password, string firstName, string lastName, string gender, int roleID = 0)
        {
            UserProfileView UPV = new UserProfileView();
            UPV.UserId = userID;
            UPV.EmailAddress = emailid;
           
            UPV.FirstName = firstName;
            UPV.LastName = lastName;
            

            if (roleID > 0)
                UPV.UserLookupRoleId = roleID;

            UserManager UM = new UserManager();
            UM.UpdateUserAccount(UPV);

            return Json(new { success = true });
        }

        //[AuthorizeRoles("Admin")]
        public ActionResult DeleteUser(int userID)
        {
            UserManager UM = new UserManager();
            UM.DeleteUser(userID);
            return Json(new { success = true });
        }

        //[Authorize]
        public ActionResult EditProfile()
        {
            string loginName = User.Identity.Name;
            UserManager UM = new UserManager();
            UserProfileView UPV = UM.GetUserProfile(UM.GetUserID(loginName));
            return View(UPV);
        }


        [HttpPost]
        //[Authorize]
        public ActionResult EditProfile(UserProfileView profile)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                UM.UpdateUserAccount(profile);

                ViewBag.Status = "Update Sucessful!";
            }
            return View(profile);
        }

    }
}