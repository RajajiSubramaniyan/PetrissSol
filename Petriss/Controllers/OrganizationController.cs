using Petriss.Models.DB;
using Petriss.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Petriss.Controllers
{
    public class OrganizationController : Controller
    {
        PetrissEntities _PetrissEntities =new PetrissEntities();
        [HttpGet]
        public ActionResult Index()
        {
          //  return View(_PetrissEntities.Organizations.ToList());
            return View();
        }
        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            //Note : you can bind same list from database  
            List<OrganizationList> ObjList = new List<OrganizationList>()
            {

                new OrganizationList {OrganizationId=1,OrganizationName="Latur Chemical" },
                new OrganizationList {OrganizationId=2,OrganizationName="Texas Instruments" },
                new OrganizationList {OrganizationId=3,OrganizationName="WashingTon Soft" },
                new OrganizationList {OrganizationId=4,OrganizationName="California" },
                new OrganizationList {OrganizationId=5,OrganizationName="Auston" },
                new OrganizationList {OrganizationId=6,OrganizationName="NewYork" },
                new OrganizationList {OrganizationId=7,OrganizationName="New Chemicals" }

        };
            //Searching records from list using LINQ query  
            var OrganizationName = (from N in ObjList
                            where N.OrganizationName.StartsWith(Prefix)
                            select new { N.OrganizationName });
            return Json(OrganizationName, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult AddNewOrganization()
        {
            return View();
        }
    }
}