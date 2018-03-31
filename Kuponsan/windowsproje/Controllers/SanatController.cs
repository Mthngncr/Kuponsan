using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using windowsproje.Models;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace windowsproje.Controllers
{
    public class SanatController : Controller
    {
        private kupansandbEntities1 db = new kupansandbEntities1();
        // GET: Guzellik
        public ActionResult Index(int? page)
        {

            var kuponlar = from s in db.Kuponlar
                           select s;
            kuponlar = kuponlar.OrderByDescending(s => s.KayıtTarih);

            kuponlar = kuponlar.Where(s => s.KategoriID.Equals(5));
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(kuponlar.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult kuponDetay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kuponlar kuponlar = db.Kuponlar.Find(id);
            if (kuponlar == null)
            {
                return HttpNotFound();
            }
            return View(kuponlar);
        }
    }
}