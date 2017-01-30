using Petriss.Models.DB;
using Petriss.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Petriss.Controllers
{
    public class VendorController : Controller
    {
        PetrissEntities _PetrissEntities =new PetrissEntities();
        [HttpGet]
        public ActionResult Index()
        {
          //  return View(_PetrissEntities.Vendors.ToList());
            return View();
        }
        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            //Note : you can bind same list from database  
            List<VendorList> ObjList = new List<VendorList>()
            {

                new VendorList {VendorId=1,VendorName="Latur Chemical" },
                new VendorList {VendorId=2,VendorName="Texas Instruments" },
                new VendorList {VendorId=3,VendorName="WashingTon Soft" },
                new VendorList {VendorId=4,VendorName="California" },
                new VendorList {VendorId=5,VendorName="Auston" },
                new VendorList {VendorId=6,VendorName="NewYork" },
                new VendorList {VendorId=7,VendorName="New Chemicals" }

        };
            //Searching records from list using LINQ query  
            var VendorName = (from N in ObjList
                            where N.VendorName.StartsWith(Prefix)
                            select new { N.VendorName });
            return Json(VendorName, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult AddNewVendor()
        {
            return View();
        }
    }
}