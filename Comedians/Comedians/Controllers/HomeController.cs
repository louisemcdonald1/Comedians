using Comedians.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Comedians.Controllers
{
    public class HomeController : Controller
    {
        private ComedyEntities db = new ComedyEntities();

        //Index action method and associated action methods--------------------------------------------------------
        public ActionResult Index()
        {
            var comedians = from c in db.Performers
                            select c;

            return View(comedians);
        }

        //used by Index view
        public ActionResult _TableRow(int id)
        {
            Performer comedian = db.Performers.Find(id);
            return PartialView(comedian);
        }

        //used by Index view
        [HttpPost]
        public ActionResult _Like(int id)
        {
            Performer comedian = db.Performers.Find(id);

            int currentLikes = (int)comedian.Likes;
            comedian.Likes = currentLikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(comedian).State = EntityState.Modified;
                db.SaveChanges();
            }

            comedian = db.Performers.Find(id);

            return PartialView("_TableRow", comedian);
        }

        //used by Index view
        [HttpPost]
        public ActionResult _Dislike(int id)
        {
            Performer comedian = db.Performers.Find(id);

            int currentDislikes = (int)comedian.Dislikes;
            comedian.Dislikes = currentDislikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(comedian).State = EntityState.Modified;
                db.SaveChanges();
            }

            comedian = db.Performers.Find(id);

            return PartialView("_TableRow", comedian);
        }
        //end of action methods associated with the Index action method -----------------------------------------------

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Performer comedian)
        {
            //set initial values for likes and dislikes
            comedian.Likes = 0;
            comedian.Dislikes = 0;

            //set a default value for picture if it's null
            if (comedian.PictureUrl == null)
            {
                comedian.PictureUrl = "Content\\laughter.jpg";
            }

            db.Performers.Add(comedian);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Performer comedian = db.Performers.Find(id);

            return View(comedian);
        }

        //for releasing resources that are finished with
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }//end class
}//end namespace