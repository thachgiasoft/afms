using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace WebAegisFms.Controllers
{
    public class AboutUsController : Controller
    {
        [HttpGet]
        public ActionResult About()
        {
            AboutUs aboutUs = new AboutUs();
            //AboutUsInfo returnValue = aboutUs.Get(new AboutUsInfo());
            AboutUsInfo returnValue = new AboutUsInfo();
            return View(returnValue);
        }

        [HttpPost]
        public ActionResult About(AboutUsInfo model)
        {
            AboutUs aboutUs = new AboutUs();
            bool returnVal = true;
            if (ModelState.IsValid)
            {
                if (model.AboutUsId > 0)
                {
                    returnVal = aboutUs.Set(model);
                }
                else
                {
                    returnVal = aboutUs.Insert(model);
                }
            }

            return this.Json(returnVal, JsonRequestBehavior.AllowGet);
        }
       

    }
}
