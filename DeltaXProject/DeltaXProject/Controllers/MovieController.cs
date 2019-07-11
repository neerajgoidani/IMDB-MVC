using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeltaXProject.DAL;
using DeltaXProject.Models;

namespace DeltaXProject.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationContext db = new ApplicationContext();


        public ActionResult Index()
        {
            var movies = db.Movies.Include(m => m.Producer);
            return View(movies.ToList());
        }

        /// <summary>
        ///To retrieve an image from db for a movie. async because it might take a little while
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> RetrieveImage(int id)
        {
                Movie item = await db.Movies.FindAsync(id);

                byte[] photoBack = item.Poster;

                return File(photoBack, "image/png");
  
        }


        /// <summary>
        /// To get the details of a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movies.Find(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                return View(movie);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }


           
        }





        /// <summary>
        /// Adding a new actor
        /// </summary>
        /// <param name="movieVM">Custom model via which producer is added<</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddActor(MovieViewModel movieVM)
        {
            try
            {



                if (ModelState.IsValid)
                {
                    int flag = 0; // to indicate if something is wrong
                    bool exists = CheckIfActorExists(movieVM.Actor);
                    if (exists)
                    {
                        ViewBag.Error = "Actor with same name exists.";
                        flag = 1;
                    }
                    bool gender = CheckIfGenderIsCorrect(movieVM.Actor.Sex);
                    if (!gender)
                    {
                        ViewBag.Error = "Sex has to be male or female.";
                        flag = 1;
                    }


                    if (flag == 1)
                    {

                        ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName");
                        int[] SelectedActorId = new int[] { 1 };
                        ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                        List<SelectListItem> actorList = new List<SelectListItem>();
                        foreach (var actor in db.Actors)
                        {
                            actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                        }
                        ViewBag.ActorList = actorList;
                        ViewBag.actorToBeAdded = true;
                        return View("Create");

                    }
                    else
                    {

                        var actorToAdd = new Actor
                        {
                            ActorName = movieVM.Actor.ActorName,
                            Dob = movieVM.Actor.Dob,
                            Bio = movieVM.Actor.Bio,
                            Sex = movieVM.Actor.Sex
                        };
                        db.Actors.Add(actorToAdd);
                        db.SaveChanges();
                        return RedirectToAction("Create");




                    }





                }
                else
                {
                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName");
                    int[] SelectedActorId = new int[] { 1 };
                    ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                    List<SelectListItem> actorList = new List<SelectListItem>();
                    foreach (var actor in db.Actors)
                    {
                        actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                    }
                    ViewBag.ActorList = actorList;
                    ViewBag.Error = "Model was not valid.";
                    ViewBag.actorToBeAdded = true;
                    return View("Create");
                }


            }
            catch (Exception e)
            {
                ViewBag.Error = "Some unexpected error occured. Try again.";
                return View("Index");
            }




        }
        /// <summary>
        /// Adding a new producer
        /// </summary>
        /// <param name="movieVM">Custom model via which producer is added</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProducer(MovieViewModel movieVM)
        {
            try
            {
                int flag = 0; // to indicate something is wrong

                if (ModelState.IsValid)
                {
                    bool exists = CheckIfProducerExists(movieVM.Producer);
                    if (exists)
                    {
                        ViewBag.Error = "Producer with same name exists.";
                        flag = 1;
                    }
                    bool gender = CheckIfGenderIsCorrect(movieVM.Producer.Sex);
                    if (!gender)
                    {
                        ViewBag.Error = "Sex has to be male or female.";
                        flag = 1;
                    }



                    if (flag == 1)
                    {

                        ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName");
                        int[] SelectedActorId = new int[] { 1 };
                        ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                        List<SelectListItem> actorList = new List<SelectListItem>();
                        foreach (var actor in db.Actors)
                        {
                            actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                        }
                        ViewBag.ActorList = actorList;
                        ViewBag.producerToBeAdded = true;
                        return View("Create");

                    }
                    else
                    {
                        var producerToAdd = new Producer
                        {
                            ProducerName = movieVM.Producer.ProducerName,
                            Dob = movieVM.Producer.Dob,
                            Bio = movieVM.Producer.Bio,
                            Sex = movieVM.Producer.Sex
                        };
                        db.Producers.Add(producerToAdd);
                        db.SaveChanges();
                        return RedirectToAction("Create");

                    }
                }
                else
                {
                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName");
                    int[] SelectedActorId = new int[] { 1 };
                    ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                    List<SelectListItem> actorList = new List<SelectListItem>();
                    foreach (var actor in db.Actors)
                    {
                        actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                    }
                    ViewBag.ActorList = actorList;
                    ViewBag.Error = "Model was not valid.";
                    ViewBag.producerToBeAdded = true;
                    return View("Create");
                }


            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }


           

        }


        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <param name="actorAdded">A parameter to show if an actor has to be added</param>
        /// <param name="producerAdded">A parameter to show if a producer has to be added</param>
        /// <returns></returns>
        public ActionResult Create(bool? actorAdded, bool? producerAdded)
        {
            try
            {

                MovieViewModel movieViewModel = new MovieViewModel();
                movieViewModel.Movie = new Movie();
                movieViewModel.Producer = new Producer();
                movieViewModel.Actor = new Actor();

                ViewBag.actorToBeAdded = false;
                ViewBag.producerToBeAdded = false;
                if (actorAdded == true)
                {
                    ViewBag.actorToBeAdded = true;
                }
                if (producerAdded == true)
                {
                    ViewBag.producerToBeAdded = true;
                }

                int[] SelectedActorId = new int[] { 1 };
                ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                List<SelectListItem> actorList = new List<SelectListItem>();
                foreach (var actor in db.Actors)
                {
                    actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                }
                ViewBag.ActorList = actorList;
                ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName");
                return View(movieViewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }



        }

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <param name="movieVM">model for the movie</param>
        /// <param name="form"></param>
        /// <returns></returns>    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieViewModel movieVM, FormCollection form)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    bool exists = CheckIfMovieExists(movieVM.Movie);
                    if (exists)
                    {
                        ViewBag.Error = "Movie with same name exists.";
                        ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movieVM.Movie.ProducerID);
                        int[] SelectedActorId = new int[] { 1 };
                        ViewBag.SelectedActorId = SelectedActorId; // setting it as 1 as we cannot not select an actor
                        List<SelectListItem> actorList = new List<SelectListItem>();
                        foreach (var actor in db.Actors)
                        {
                            actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                        }
                        ViewBag.ActorList = actorList;
                        return View(movieVM);
                    }
                    else
                    {
                        string selectedActorIds = form["SelectedActorId"];
                        string[] selectedActorIdsArray = selectedActorIds.Split(',');
                        HttpPostedFileBase file = Request.Files["Poster"];
                        byte[] image = UploadImageInDataBase(file);

                        var movieToCreate = new Movie
                        {
                            MovieName = movieVM.Movie.MovieName,
                            YearOfRelease = movieVM.Movie.YearOfRelease,
                            Plot = movieVM.Movie.Plot,
                            ProducerID = int.Parse(form["ProducerID"]),
                            Poster = image
                        };
                        UpdateMovieActors(selectedActorIdsArray, movieToCreate);
                        db.Movies.Add(movieToCreate);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }


                }
                else
                {
                    int[] SelectedActorId = new int[] { 1 };
                    ViewBag.SelectedActorId = SelectedActorId; // setting it as 0 as index automatically starts from 1.
                    List<SelectListItem> actorList = new List<SelectListItem>();
                    foreach (var actor in db.Actors)
                    {
                        actorList.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                    }
                    ViewBag.ActorList = actorList;
                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movieVM.Movie.ProducerID);
                    return View(movieVM);
                }

                
            }
            catch (Exception e)
            {
                ViewBag.Error = "Some unexpected error occured. Try again.";
                return View("Index");
            }



        }

        [NonAction]
        private bool CheckIfMovieExists(Movie movie)
        {

            if (db.Movies.Any(m => m.MovieName.Equals(movie.MovieName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }

        [NonAction]
        private bool CheckIfActorExists(Actor actor)
        {

            if (db.Actors.Any(a => a.ActorName.Equals(actor.ActorName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }


        [NonAction]
        private bool CheckIfProducerExists(Producer producer)
        {
            
                if (db.Producers.Any(p => p.ProducerName.Equals(producer.ProducerName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return true;
                }
                return false;
                       
            
        }

        [NonAction]
        private bool CheckIfGenderIsCorrect(string gender)
        {          
                if (gender.Equals("Male", StringComparison.CurrentCultureIgnoreCase) || gender.Equals("Female", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                return false;

        }


        /// <summary>
        /// To upload the image/poster of a movie in a database
        /// </summary>
        /// <param name="file">the image to be copied</param>
        /// <returns></returns>
        [NonAction]
        private byte[] UploadImageInDataBase(HttpPostedFileBase file)
        {
            
                byte[] imageBytes = null;
                BinaryReader reader = new BinaryReader(file.InputStream);
                imageBytes = reader.ReadBytes((int)file.ContentLength);
                return imageBytes;
            
        }

        /// <summary>
        /// Movie to be edited
        /// </summary>
        /// <param name="id">id of movie to be edited</param>
        /// <returns></returns>
        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movies.Find(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }


                var actorsIdList = movie.Actors.Select(a => a.ID);
                ViewBag.movieActorsSelectedId = actorsIdList;

                var totalActorList = db.Actors;
                ViewBag.totalActorList = new SelectList(totalActorList, "ActorID", "ActorName");
                int[] SelectedValues = actorsIdList.ToArray();
                ViewBag.SelectedValues = SelectedValues;
                List<SelectListItem> values = new List<SelectListItem>();
                foreach (var actor in db.Actors)
                {
                    values.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                }

                ViewBag.Values = values;
                ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movie.ProducerID);
                return View(movie);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }



        /// <summary>
        /// Movie data to edited
        /// </summary>
        /// <param name="movieModel">the movie which has to be edited</param>
        /// <param name="form"></param>
        /// <returns></returns>
        // POST: Movie/Edit/5     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,MovieName,YearOfRelease,Plot,Poster,ProducerID")] Movie movieModel, FormCollection form)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool exists = CheckIfMovieExists(movieModel);
                    if (exists)
                    {
                        ViewBag.Error = "Movie with same name exists.";
                        ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movieModel.ProducerID);
                        var totalActorList = db.Actors;
                        ViewBag.totalActorList = new SelectList(totalActorList, "ActorID", "ActorName");

                        string selectednewActorIds = form["SelectedValues"];
                        string[] newActorIds = selectednewActorIds.Split(',');

                        int[] SelectedValues = Array.ConvertAll(newActorIds, s => int.Parse(s));
                        ViewBag.SelectedValues = SelectedValues;

                        List<SelectListItem> values = new List<SelectListItem>();
                        foreach (var actor in db.Actors)
                        {
                            values.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                        }

                        ViewBag.Values = values;
                        ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movieModel.ProducerID);
                     
                        return View(movieModel);
                    }
                    else
                    {

                        string selectednewActorIds = form["SelectedValues"];
                        string[] newActorIds = selectednewActorIds.Split(',');

                        HttpPostedFileBase file = Request.Files["Poster"];

                        var movieToUpdate = db.Movies.Single(m => m.MovieID == movieModel.MovieID);

                        movieToUpdate.MovieName = movieModel.MovieName;
                        movieToUpdate.YearOfRelease = movieModel.YearOfRelease;
                        movieToUpdate.Plot = movieModel.Plot;
                        movieToUpdate.ProducerID = movieModel.ProducerID;
                        UpdateMovieActors(newActorIds, movieToUpdate);

                        if (file != null)
                        {
                            byte[] image = UploadImageInDataBase(file);
                            movieToUpdate.Poster = image;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }



                }
                else
                {
                    ViewBag.Error = "Movie could not be edited. Some error occured in the model.";
                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", movieModel.ProducerID);
                    

                    var totalActorList = db.Actors;
                    ViewBag.totalActorList = new SelectList(totalActorList, "ActorID", "ActorName");


                    string selectednewActorIds = form["SelectedValues"];
                    string[] newActorIds = selectednewActorIds.Split(',');

                    int[] SelectedValues = Array.ConvertAll(newActorIds, s => int.Parse(s));
                    ViewBag.SelectedValues = SelectedValues;
                   
                    List<SelectListItem> values = new List<SelectListItem>();
                    foreach (var actor in db.Actors)
                    {
                        values.Add(new SelectListItem { Value = actor.ID.ToString(), Text = actor.ActorName });
                    }

                    ViewBag.Values = values;
                    return View(movieModel);

                }
                
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }



        }

        /// <summary>
        /// Updates the actors for a particular movie
        /// </summary>
        /// <param name="newActorIds">list of new actor ids</param>
        /// <param name="movieToUpdate">the movie whos actors have to be updated</param>
        [NonAction]
        public void UpdateMovieActors(string[] newActorIds, Movie movieToUpdate)
        {
            try
            {
                if (newActorIds == null)
                {
                    movieToUpdate.Actors = new List<Actor>(); // updating it with empty actors
                    return;
                }

                var initalMovieActorList = movieToUpdate.Actors;
                var newMovieActorList = new List<Actor>();

                foreach (string id in newActorIds)
                {
                    Actor actor = db.Actors.Where(y => y.ID.ToString() == id).Single();
                    newMovieActorList.Add(actor);

                }
                movieToUpdate.Actors = newMovieActorList;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }



           
        }

        //[NonAction]
        //public void AddMovieActors(string[] newActorIds, Movie movieToUpdate)
        //{
        //    if (newActorIds == null)
        //    {
        //        movieToUpdate.Actors = new List<Actor>(); // updating it with empty actors
        //        return;
        //    }

        //    var initalMovieActorList = movieToUpdate.Actors;
        //    var newMovieActorList = new List<Actor>();

        //    foreach (string x in newActorIds)
        //    {
        //        Actor actor = db.Actors.Where(y => y.ID.ToString() == x).Single();
        //        newMovieActorList.Add(actor);

        //    }
        //    movieToUpdate.Actors = newMovieActorList;
        //    db.SaveChanges();

        //}



        /// <summary>
        /// Deletes a movie 
        /// </summary>
        /// <param name="id">id of the movie to be deleted</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movies.Find(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                return View(movie);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }



            
        }

        /// <summary>
        /// Deletes a movie
        /// </summary>
        /// <param name="id">id of the movie to be deleted</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Movie movie = db.Movies.Find(id);
                db.Movies.Remove(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index");
            }



            
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
