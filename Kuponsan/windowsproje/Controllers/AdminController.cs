using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using windowsproje.Models;

namespace windowsproje.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(Kullanıcılar u)
        {
            if (ModelState.IsValid)
            {
                using (kupansandbEntities1 dc = new kupansandbEntities1())
                {
                    var v = dc.Kullanıcılar.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password) && a.Yetki.Equals(1)).FirstOrDefault();

                    if (v != null)
                    {
                        Session["LogedUserID"] = v.Id.ToString();
                        Session["LogedUsername"] = v.Username.ToString();
                        Session["LoggedFullName"] = v.FullName.ToString();
                        return RedirectToAction("Index", "AfterLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                    }
                }


            }

            return View(u);
        }

        public ActionResult AfterLogin()
        {
            if (Session["LogedUsername"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Admin");
            }

        }

        public ActionResult ForgotPassword()
        {
            return RedirectToAction("Index", "ForgotPassword");
        }



    }
}