using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Text;

namespace WebAegisFms.Controllers
{
    public class ContactUsController : Controller
    {
        //
        // GET: /ContactUs/

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpGet]
        public HtmlString GetContactUsHTML(ContactUsInfo cInfo)
        {
            StringBuilder str = new StringBuilder("");

            ContactUs contactUs = new ContactUs();
            
            return new HtmlString(str.ToString());
        }



    }
}
