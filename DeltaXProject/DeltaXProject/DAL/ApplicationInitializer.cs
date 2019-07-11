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
            new Actor{ActorName="Salman",Sex="Male",Dob=DateTime.Parse("2005-09-01"),Bio="Bleh"},
            new Actor{ActorName="Mansi",Sex="Female",Dob=DateTime.Parse("2002-09-01"),Bio="Bleh"},
            new Actor{ActorName="Vishwas",Sex="Male",Dob=DateTime.Parse("2003-09-01"),Bio="Bleh"},

            };

            actors.ForEach(s => context.Actors.Add(s));
            context.SaveChanges();

            var producers = new List<Producer>()
            {
                new Producer{ProducerName="Aatish",Sex="Male",Bio="Bleh",Dob=DateTime.Parse("1991-01-01")},
                new Producer{ProducerName="Mahek",Sex="Female",Bio="Bleh",Dob=DateTime.Parse("2001-01-01")},
                new Producer{ProducerName="Arnav",Sex="Male",Bio="Blehdf",Dob=DateTime.Parse("2011-01-01")},

            };
            producers.ForEach(p => context.Producers.Add(p));
            context.SaveChanges();

            var movies = new List<Movie>
            {
            new Movie{Plot="Horror",MovieName="CheckMate",YearOfRelease=DateTime.Parse("2005-09-01"),ProducerID=1,Actors = new List<Actor>()},
            new Movie{Plot="Adventure",MovieName="LOTR",YearOfRelease=DateTime.Parse("2002-09-01"),ProducerID=2,Actors = new List<Actor>()},
            new Movie{Plot="Adventure",MovieName="Harry Potter",YearOfRelease=DateTime.Parse("2001-09-01"),ProducerID=3,Actors = new List<Actor>()},

            };
            movies.ForEach(s => context.Movies.Add(s));
            context.SaveChanges();

            AddOrUpdateActor(context, "CheckMate", "Salman");
            AddOrUpdateActor(context, "CheckMate", "Mansi");
            AddOrUpdateActor(context, "LOTR", "Salman");
            AddOrUpdateActor(context, "LOTR", "Vishwas");

            AddOrUpdateActor(context, "Harry Potter", "Salman");
            AddOrUpdateActor(context, "Harry Potter", "Mansi");
            AddOrUpdateActor(context, "Harry Potter", "Vishwas");

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