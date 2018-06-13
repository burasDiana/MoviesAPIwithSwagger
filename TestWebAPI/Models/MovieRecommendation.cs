using DataAccess;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class MovieRecommendation: IExamplesProvider
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public List<Movy> recommendedMvs { get; set; }

        public string matchScore { get; set; }

        public object GetExamples()
        {
            string date = "2001-05-09";
            return new List<MovieRecommendation>
            {
                new MovieRecommendation
                { ID = 1, Title = "Star Warts", matchScore = "76%", recommendedMvs = new List<Movy>(){
                new Movy {ID = 3, Genre = "Comedy" ,Price = 11, ReleaseDate = Convert.ToDateTime(date), Title = "Star Wars 2" },
                new Movy {ID = 4, Genre = "Action" ,Price = 10, ReleaseDate = Convert.ToDateTime(date), Title = "Star Wars 3" }
                }}
            };
        }
    }
}