using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Petriss.Models;
using Petriss.Models.ViewModel;
using System.Web.Security;
using Petriss.Models.EntityManager;
using System.Net.Mail;
using System.Net;

namespace Petriss.Controllers
{
    
    public class AccountController : Controller
    {
          
      
            public ActionResult SignUp()
            {
                return View();
            }

            [HttpPost]
            public ActionResult SignUp(UserSignUpView USV)
            {
                if (ModelState.IsValid)
                {
                    UserManager UM = new UserManager();
                    if (!UM.IsLoginNameExist(USV.EmailAddress))
                    {
                        UM.AddUserAccount(USV);
                        
                        FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                        return RedirectToAction("Welcome", "Home");

                    }
                    else
                        ModelState.AddModelError("", "Login Name already taken.");
                }
                return View();
            }

            [Authorize]
            public ActionResult SignOut()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }


            public ActionResult LogIn()
            {
                return View();
            }

            [HttpPost]
            public ActionResult LogIn(UserLoginView ULV, string returnUrl)
            {
            string _url = string.Empty;
          
                if (ModelState.IsValid)
                {

                //if (ULV.EmailAddress== Request.Url.GetLeftPart(UriPartial.Path))
                //{
                    UserManager UM = new UserManager();
                    string password = UM.GetUserPassword(ULV.EmailAddress);

                    if (string.IsNullOrEmpty(password))
                        ModelState.AddModelError("", "The user login or password provided is incorrect.");
                    else
                    {
                        if (ULV.Password.Equals(password))
                        {
                            //FormsAuthentication.SetAuthCookie(ULV.LoginName, false);
                            //return RedirectToAction("Welcome", "Home");
                             FormsAuthentication.RedirectFromLoginPage(ULV.EmailAddress, false);
                        }
                        else
                        {
                            ModelState.AddModelError("", "The password provided is incorrect.");
                        }
                    }
                //}
                //else
                //{
                //    ModelState.AddModelError(" ","Please Check your email and activate/login via Url");
                //}
                }

                // If we got this far, something failed, redisplay form
                return View(ULV);
            }
      
        private void Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("recipient@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("sender@outlook.com");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "user@outlook.com",  // replace with valid value
                        Password = "password"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;                   
                }
            }
           
        }

      
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}