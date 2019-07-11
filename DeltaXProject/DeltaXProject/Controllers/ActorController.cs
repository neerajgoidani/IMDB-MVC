using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeltaXProject.DAL;
using DeltaXProject.Models;

namespace DeltaXProject.Controllers
{
    public class ActorController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: List of all actors
        public ActionResult Index()
        {
            try
            {
                return View(db.Actors.ToList());
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }

        }

        // GET: Details of an Actor
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actors.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // GET: Create a new Actor
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create a new Actor      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ActorName,Sex,Dob,Bio")] Actor actor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool exists = CheckIfActorExists(actor,"Create");


                    if (exists)
                    {
                        ViewBag.Error = "Actor with same name already exists";
                        return View(actor);
                    }
                    else if (!CheckIfGenderIsCorrect(actor.Sex))
                    {
                        ViewBag.Error = "Please enter male or female as gender.";
                        return View(actor);

                    }
                    else
                    {

                        db.Actors.Add(actor);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }
                return View(actor);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }




        }

        // GET: Edit an Actor
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actors.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // POST: Edit an Actor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ActorName,Sex,Dob,Bio")] Actor actor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool exists = CheckIfActorExists(actor,"Edit");


                    if (exists)
                    {
                        ViewBag.Error = "Actor with same name already exists";
                        return View(actor);
                    }
                    else if (!CheckIfGenderIsCorrect(actor.Sex))
                    {
                        ViewBag.Error = "Please enter male or female as gender.";
                        return View(actor);

                    }
                    else

                    {
                        db.Entry(actor).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                }
                return View(actor);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // GET: Delete an Actor
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actors.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // POST: Delete an Actor
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Actor actor = db.Actors.Find(id);
                db.Actors.Remove(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        /// <summary>
        /// Check if a actor with the same name exists
        /// </summary>
        /// <param name="producer">new actor</param>
        /// <returns>true if a actor with the same name exists</returns>
        [NonAction]
        private bool CheckIfActorExists(Actor actor, string mode)
        {
            int count = db.Actors.Count(a => a.ActorName.Equals(actor.ActorName, StringComparison.CurrentCultureIgnoreCase));
            if (mode.Equals("Create"))
            {
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            else if (mode.Equals("Edit"))
            {
                if (count > 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check if gender is male or female
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>true if gender is male or female.</returns>
        [NonAction]
        private bool CheckIfGenderIsCorrect(string gender)
        {
            if (gender.Equals("Male", StringComparison.CurrentCultureIgnoreCase) || gender.Equals("Female", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
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
