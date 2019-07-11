using DeltaXProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeltaXProject.DAL
{
    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {

        protected override void Seed(ApplicationContext context)
        {

            var actors = new List<Actor>
            {
            new Actor{ActorName="Gautam",Sex="Male",Dob=DateTime.Parse("1996-09-01"),Bio="A veteran in this industry."},
            new Actor{ActorName="Mansi",Sex="Female",Dob=DateTime.Parse("1989-09-05"),Bio="A famous bollywood heroine"},
            new Actor{ActorName="Vishwas",Sex="Male",Dob=DateTime.Parse("2001-10-03"),Bio="South Indian actor."},

            };

            actors.ForEach(s => context.Actors.Add(s));
            context.SaveChanges();

            var producers = new List<Producer>()
            {
                new Producer{ProducerName="Aatish",Sex="Male",Bio="Well known In Bollywood. Has 20 years experience",Dob=DateTime.Parse("1980-04-04")},
                new Producer{ProducerName="Mahek",Sex="Female",Bio="Just graduated from a film making college",Dob=DateTime.Parse("1995-01-01")},
                new Producer{ProducerName="Arnav",Sex="Male",Bio="Assisant Director in 3 100 Cr+ grossing film.",Dob=DateTime.Parse("1988-03-08")},

            };
            producers.ForEach(p => context.Producers.Add(p));
            context.SaveChanges();

            var movies = new List<Movie>
            {
            new Movie{Plot="A college friends trip is a must",MovieName="Zindagi Na Milegi Dobaara",YearOfRelease=DateTime.Parse("2005-08-08"),ProducerID=1,Actors = new List<Actor>()},
            new Movie{Plot="Reality vs Imagination in IT",MovieName="Life at a Tech Giant",YearOfRelease=DateTime.Parse("2002-09-01"),ProducerID=2,Actors = new List<Actor>()},
            new Movie{Plot="Autobiography of a market disrupter",MovieName="Story of DeltaX",YearOfRelease=DateTime.Parse("2001-03-10"),ProducerID=3,Actors = new List<Actor>()},

            };
            movies.ForEach(s => context.Movies.Add(s));
            context.SaveChanges();

            AddOrUpdateActor(context, "Zindagi Na Milegi Dobaara", "Gautam");
            AddOrUpdateActor(context, "Zindagi Na Milegi Dobaara", "Mansi");
            AddOrUpdateActor(context, "Life at a Tech Giant", "Gautam");
            AddOrUpdateActor(context, "Life at a Tech Giant", "Vishwas");

            AddOrUpdateActor(context, "Story of DeltaX", "Gautam");
            AddOrUpdateActor(context, "Story of DeltaX", "Mansi");
            AddOrUpdateActor(context, "Story of DeltaX", "Vishwas");

            context.SaveChanges();


           


        }

        void AddOrUpdateActor(ApplicationContext context, string movieName, string actorName)
        {
            var movie = context.Movies.SingleOrDefault(m => m.MovieName == movieName); // get a movie by its id
            var inst = movie.Actors.SingleOrDefault(i => i.ActorName == actorName); // get the first actor for that movie
            if (inst == null)
                movie.Actors.Add(context.Actors.Single(i => i.ActorName == actorName)); // Actor.Add() is possible coz we have created a list above
        }
    }
}