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
    public class ProducerController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: List of Producers
        public ActionResult Index()
        {
            try
            {
                return View(db.Producers.ToList());
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }

        }

        // GET: Details of a Producer
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producers.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // GET: Create a new Producer
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // POST: Create a new Producer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProducerID,ProducerName,Sex,Dob,Bio")] Producer producer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool exists = CheckIfProducerExists(producer);


                    if (exists)
                    {
                        ViewBag.Error = "Actor with same name already exists";
                        return View(producer);
                    }
                    else if (!CheckIfGenderIsCorrect(producer.Sex))
                    {
                        ViewBag.Error = "Please enter male or female as gender.";
                        return View(producer);

                    }


                    db.Producers.Add(producer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(producer);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // GET: Edit a Producer
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producers.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // POST: Edit a Producer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProducerID,ProducerName,Sex,Dob,Bio")] Producer producer)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool exists = CheckIfProducerExists(producer);


                    if (exists)
                    {
                        ViewBag.Error = "Actor with same name already exists";
                        return View(producer);
                    }
                    else if (!CheckIfGenderIsCorrect(producer.Sex))
                    {
                        ViewBag.Error = "Please enter male or female as gender.";
                        return View(producer);

                    }



                    db.Entry(producer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(producer);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }

        }

        // GET: Delete a Producer
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producers.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Something unexpected happened.";
                return View("Index");
            }


        }

        // POST: Delete a Producer
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Producer producer = db.Producers.Find(id);
                db.Producers.Remove(producer);
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
        /// Check if a producer with the same name exists
        /// </summary>
        /// <param name="producer">new producer</param>
        /// <returns>true if a producer with the same name exists</returns>
        [NonAction]
        private bool CheckIfProducerExists(Producer producer)
        {

            if (db.Producers.Any(p => p.ProducerName.Equals(producer.ProducerName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
            return false;
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
