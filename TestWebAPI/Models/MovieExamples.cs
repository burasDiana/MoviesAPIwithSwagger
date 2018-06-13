using DataAccess;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class MovieExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            string date = "2001-05-09";
            //string date2 = "2003-04-10";
            return new List<Movy>
           {
               new Movy {ID = 1, Genre = "Action" ,Price = 10, ReleaseDate = Convert.ToDateTime(date), Title = "John Wick 2" }
               //new Movie {ID = 2, Genre = "Comedy" ,Price = 8, ReleaseDate = Convert.ToDateTime(date2), Title = "Deadpool" }
           };
        }
    }
}