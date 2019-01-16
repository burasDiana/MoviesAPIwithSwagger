using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebAPI.Controllers;
using TestWebAPI.Models;

namespace TestWebAPI.Tests.Controllers
{
    [TestClass]
    public class TestMovieController
    {
        private MoviesV2Controller controller;
        private TestMovieAppContext context;

        [TestInitialize]
        public void GetDbContext()
        {
            controller = new MoviesV2Controller(new TestMovieAppContext());
            context = new TestMovieAppContext();
        }

        [TestMethod]
        public void PostMovie_ShouldReturnMoviewithSameId()
        {
            var item = GetDemoMovie();
            var result = controller.PutMovie(item.ID, item) as StatusCodeResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void GetMovie_ShouldReturnMoviewithSameId()
        {
            context.Movies.Add(GetDemoMovie());
            var ctrl = new MoviesV2Controller(context);
            var result = ctrl.GetMovie(1) as OkNegotiatedContentResult<Movie>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Content.ID);
        }

        Movie GetDemoMovie()
        {
            return new Movie()
                {ID = 1, Price = 12, ReleaseDate = new DateTime(2000, 05, 30), Genre = "Drame", Title = "Room"};
        }
    }
}
