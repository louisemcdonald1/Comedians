using Comedians.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Comedians.Controllers
{
    public class HomeController : Controller
    {
        private ComedyEntities db = new ComedyEntities();

        //Index action method and associated action methods--------------------------------------------------------
        public ActionResult Index(string genres, string searchString, string sortOrder)
        {
            //get data for drop-down list of genres in Index view---------------------------

            List<string> genreList = new List<string>();
            //get genres fron db
            var genreQuery = from g in db.Performers
                             orderby g.Genre
                             select g.Genre;
            //put unique genres in list and put list in ViewBag
            genreList.AddRange(genreQuery.Distinct());
            ViewBag.Genres = new SelectList(genreList);

            //sorting parameters-----------------------------------------------------------------
            //put opposite sort value to the current parameter in the ViewBag, so that
            //the opposite kind of sort is made available on return to the Index view
            //the default sort type is names ascending, so this doesn't need to be specified
            ViewBag.NameSortParameter = String.IsNullOrEmpty(sortOrder) ? "nameDescending" : "";
            //make sure that the first sort on likes is descending order
            if ((sortOrder == null) || (sortOrder == "nameDescending") || (sortOrder == "nameAscending"))
            {
                ViewBag.LikesSortParameter = "likesDescending";
            }
            else if (sortOrder == "likesAscending")
            {
                ViewBag.LikesSortParameter = "likesDescending";
            }
            else
            {
                ViewBag.LikesSortParameter = "likesAscending";
            }
            

            //get all records from db ------------------------------------------------------------
            var comedians = from c in db.Performers
                            select c;

            //filter list of records according to search criteria --------------------------------
            if (!String.IsNullOrEmpty(searchString))
            {
                comedians = comedians.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(genres))
            {
                comedians = comedians.Where(g => g.Genre == genres);
            }

            //sort records according to sort criterion -------------------------------------------
            switch (sortOrder)
            {
                case "nameDescending":
                    comedians = comedians.OrderByDescending(s => s.Name);
                    break;
                case "likesAscending":
                    comedians = comedians.OrderBy(s => s.Likes);
                    break;
                case "likesDescending":
                    comedians = comedians.OrderByDescending(s => s.Likes);
                    break;
                default:
                    comedians = comedians.OrderBy(s => s.Name);
                    break;
            }

            return View(comedians);
        }

        //used by Index view to refresh rows with AJAX when likes or dislikes updated
        public ActionResult _TableRow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }
            return PartialView(comedian);
        }

        //used by Index view to update likes when likes button is clicked
        [HttpPost]
        public ActionResult _Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

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

        //used by Index view to update dislikes when dislikes button is clicked
        [HttpPost]
        public ActionResult _Dislike(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

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

        //creating records ---------------------------------------------------------------------------------------------
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
                comedian.PictureUrl = "~\\Content\\laughter.jpg";
            }

            if (ModelState.IsValid)
            {
                db.Performers.Add(comedian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comedian);
        }

        //getting details of records ------------------------------------------------------------------------------------------
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

            return View(comedian);
        }

        //editing records -----------------------------------------------------------------------------------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

            return View(comedian);
        }

        [HttpPost]
        public ActionResult Edit(Performer comedian)
        {
            //if no value for likes and dislikes, set initial values
            if (comedian.Likes == null)
            {
                comedian.Likes = 0;
            }
            if (comedian.Dislikes == null)
            {
                comedian.Dislikes = 0;
            }

            //set a default value for picture if it's null, as the Url.Content
            //helper in the views will crash if it gets a null value
            if (comedian.PictureUrl == null)
            {
                comedian.PictureUrl = "Content\\laughter.jpg";
            }
            if (ModelState.IsValid)
            {
                db.Entry(comedian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comedian);
        }

        //deleting records ----------------------------------------------------------------------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

            return View(comedian);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Performer comedian = db.Performers.Find(id);
            if (comedian == null)
            {
                return HttpNotFound();
            }

            db.Performers.Remove(comedian);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        //for releasing resources that are finished with  ----------------------------------------------------------------
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