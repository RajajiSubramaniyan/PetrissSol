using Petriss.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Petriss.Controllers
{
    public class OrganizationController : Controller
    {
        petdevEntities _PetrissEntities =new petdevEntities();

        public ActionResult Index()
        {
          //  return View(_PetrissEntities.Organizations.ToList());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddNewOrganization()
        {
            return View();
        }
    }
}