using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using windowsproje.Models;

namespace windowsproje.Controllers
{
    public class SharedController : Controller
    {
        private kupansandbEntities1 db = new kupansandbEntities1();
        // GET: Shared
        public ActionResult _Layout()
        {
            return View();
        }

    }
}