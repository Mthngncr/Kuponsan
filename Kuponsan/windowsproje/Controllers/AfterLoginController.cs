using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using windowsproje.Models;
using PagedList;

namespace windowsproje.Controllers
{
    public class AfterLoginController : Controller
    {
        private kupansandbEntities1 db = new kupansandbEntities1();

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }
        // GET: Admin
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (Session["LogedUsername"] == null)
            {
                return RedirectToAction("Index", "Admin");

            }
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var kuponlar = from s in db.Kuponlar
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    kuponlar = kuponlar.Where(s => s.KuponAd.Contains(searchString)
                                           || s.KuponDetay.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        kuponlar = kuponlar.OrderByDescending(s => s.KuponAd);
                        break;
                    case "date_asc":
                        kuponlar = kuponlar.OrderBy(s => s.KayıtTarih);
                        break;
                    case "name_asc":
                        kuponlar = kuponlar.OrderBy(s => s.KuponAd);
                        break;

                    default:  // date descending 
                        kuponlar = kuponlar.OrderByDescending(s => s.KayıtTarih);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(kuponlar.ToPagedList(pageNumber, pageSize));
            }
          
        }


        // GET: Admin/Details/5
        public ActionResult Details(int? id)
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

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KatogoriAd");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KuponID,KuponAd,KuponOzet,KuponDetay,KuponResim,Kuponİl,KuponAdres,KuponTarih,KayıtTarih,KategoriID,KuponÜcret")] Kuponlar kuponlar, HttpPostedFileBase image1)
        {

            if (ModelState.IsValid)
            {
                if (image1 != null)
                {
                    using (var reader = new System.IO.BinaryReader(image1.InputStream))
                    {
                        kuponlar.KuponResim = reader.ReadBytes(image1.ContentLength);
                    }


                }

                kuponlar.KayıtTarih = DateTime.Now;
                db.Kuponlar.Add(kuponlar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KatogoriAd", kuponlar.KategoriID);
            return View(kuponlar);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KatogoriAd", kuponlar.KategoriID);
            return View(kuponlar);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KuponID,KuponAd,KuponOzet,KuponDetay,KuponResim,Kuponİl,KuponAdres,KuponTarih,KayıtTarih,KategoriID,KuponÜcret")] Kuponlar kuponlar, HttpPostedFileBase imageedit)
        {
            if (imageedit != null)
            {
                using (var reader = new System.IO.BinaryReader(imageedit.InputStream))
                {
                    kuponlar.KuponResim = reader.ReadBytes(imageedit.ContentLength);
                }


            }


            else
            {

                var query = from s in db.Kuponlar
                            where s.KuponID.Equals(kuponlar.KuponID)
                            select s.KuponResim;
                foreach (var item in query)
                {
                    kuponlar.KuponResim = item;

                }





            }
            if (ModelState.IsValid)
            {



                kuponlar.KayıtTarih = DateTime.Now;

                db.Entry(kuponlar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KatogoriAd", kuponlar.KategoriID);
            return View(kuponlar);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kuponlar kuponlar = db.Kuponlar.Find(id);
            db.Kuponlar.Remove(kuponlar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
