using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebAPI;
using System.Net.Http;
using System.Web.Http;
using TestWebAPI.Models;
using System.Web.Http.Results;
using DataAccess;

namespace TestWebAPI.Tests.Controllers
{
    [TestClass]
    public class MovieControllerTest
    {
        /// <summary>
        /// test the title of the movie object returned matches the one in the database at id = 1
        /// </summary>
        [TestMethod]
        public void GetMovieById()
        {
            //Arrange
            var controller = new MoviesController();
            //Act
            var response = controller.GetMovie(1);
            var contentResult = response as OkNegotiatedContentResult < Movy >;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("Star Warts" , contentResult.Content.Title);
        }
        /// <summary>
        ///  test the response of the action method, when the movie is not found in the database.
        /// </summary>
        [TestMethod]
        public void GetMovieNotFound()
        {
            //Arrange
            var controller = new MoviesController();
            // Act  
            IHttpActionResult actionResult = controller.GetMovie(100);
            // Assert  
            Assert.IsInstanceOfType(actionResult , typeof(NotFoundResult));
        }

        /// <summary>
        /// test if the the objects returned are of type string
        /// </summary>
        [TestMethod]
        public void GetStrings()
        {
            //Arrange
            var controller = new MoviesController();

            //Act
            var response = controller.GetStrings();

            //Assert
            Assert.IsInstanceOfType(response[1] , typeof(string));
            Assert.AreEqual(4, response.Count);
        }
    }
}
