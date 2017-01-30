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
using System.Configuration;
using Petriss.Utilities;
using System.Drawing;
using System.IO;
using System.Text;

namespace Petriss.Controllers
{
    
    public class AccountController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AccountController));  //Declaring Log4Net

        public ActionResult SignUp()
            {
                return View();
            }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SignUp(UserSignUpView USV)
             {
           // bool _emailconfirmed = false;
            //EMAIL SERVER SETTING
            string _host = ConfigurationManager.AppSettings["host"].ToString();
            string _port = ConfigurationManager.AppSettings["port"].ToString();
            string _smptclient = ConfigurationManager.AppSettings["smtpclient"].ToString();
            string _apikey = ConfigurationManager.AppSettings["apikey"].ToString();
            string _secretkey = ConfigurationManager.AppSettings["secretkey"].ToString();
            string _emailfrom = ConfigurationManager.AppSettings["emailfrom"].ToString();
            string _emailto = ConfigurationManager.AppSettings["emailto"].ToString();

            if (ModelState.IsValid)
                {
                    UserManager UM = new UserManager();
                    if (!UM.IsLoginNameExist(USV.EmailAddress))
                    {
                    try
                    {
                        UM.AddUserAccount(USV);

                        string clientCaptcha =Request.Form["clientCaptcha"];
                        string serverCaptcha = Session["CAPTCHA"].ToString();

                        if (!clientCaptcha.Equals(serverCaptcha))
                        {
                            ViewBag.ShowCAPTCHA = serverCaptcha;

                            ViewBag.CaptchaError = "Sorry, please write exact text as written above.";
                            return View();
                        }
                        FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress(_emailfrom);
                        msg.To.Add(new MailAddress(_emailto));
                        msg.Subject = "Your Account Activation";
                        msg.Body = "<b>Dear  " + _emailto + "</b><br/>Welcome to Petriss Systems... Thanks for Registering your account.Please verify your email address by clicking the link <br/> <a href=? verifyemailcode = " + UM.GetUserActivationLink(_emailto) + ">Here</a></br/> <BR/> <B>Best Regards<br/> Petriss Systems";

                        SmtpClient client = new SmtpClient(_smptclient, Convert.ToInt32(_port));
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(_apikey, _secretkey);
                        client.Send(msg);
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }

                        return RedirectToAction("ConfirmEmail", "Account", new { Email = USV.EmailAddress });
                      }
                    else
                     {
                    ModelState.AddModelError("", "Login Name already taken.");
                }
                       
                }
                return View();
            }

            [Authorize]
            public ActionResult SignOut()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        public ActionResult LogIn()
            {
                return View();
            }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
            {
            UserManager UM = new UserManager();
            
            string activationlink =UM.GetUserActivationLink(ULV.EmailAddress);


                if (ModelState.IsValid)
                {

                if (Request.Url.AbsoluteUri == activationlink)
                {
                  
                    string password = UM.GetUserPassword(ULV.EmailAddress);

                    if (string.IsNullOrEmpty(password))
                        ModelState.AddModelError("", "The user login or password provided is incorrect.");
                    else
                    {
                        if (ULV.Password.Equals(password))
                        {
                            
                             FormsAuthentication.RedirectFromLoginPage(ULV.EmailAddress, false);
                        }
                        else
                        {
                            ModelState.AddModelError("", "The password provided is incorrect.");
                        }
                    }
                }
                else
                {
                   ModelState.AddModelError(" ","Please Check your email and activate/login via Url");
                }
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
                message.To.Add(new MailAddress(model.FromEmail));  // replace with valid value 
                message.From = new MailAddress("sender@outlook.com");  // replace with valid value
                message.Subject = "Account Activitation";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient ("smtpout.secureserver.net"))
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "shabirhakim.net",  // replace with valid value
                        Password = "Shabir@"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "webmail.shabirhakim.net";
                    smtp.Port = 25;
                    smtp.EnableSsl = true;                   
                }
            }
           
        }
        private string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }

        public ActionResult CustomCaptcha()
        {
            Session["CAPTCHA"] = GetRandomText();
            return View();
        }

        public FileResult GetCaptchaImage()
        {
            string text = Session["CAPTCHA"].ToString();

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font font = new Font("Arial", 15);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
            drawing = Graphics.FromImage(img);

            Color backColor = Color.SeaShell;
            Color textColor = Color.Red;
            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 20, 10);

            drawing.Save();

            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();

            return File(ms.ToArray(), "image/png");
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