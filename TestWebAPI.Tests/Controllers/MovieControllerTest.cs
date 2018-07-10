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
        MoviesController controller;
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new MoviesController();
        }

        /// <summary>
        /// test GetMovies returns a list of 13 movies where the first has the correct title
        /// </summary>
        [TestMethod]
        public void Should_Return_List_Of_Movies()
        {
            //Act
            var response = controller.GetMovies();
            Assert.IsNotNull(response);
            var contentResult = response as IQueryable<Movy>;
            var movieList = contentResult.ToList();
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsTrue(movieList.Count == 13);
            Assert.AreEqual("Star Warts" , movieList[0].Title);

        }
        /// <summary>
        /// test GetMovie returns the title of the movie object returned matches the one in the database at id = 1
        /// </summary>
        [TestMethod]
        public void Should_Return_Movie_With_Id()
        {
            
            //Act
            var response = controller.GetMovie(1);
            var contentResult = response as OkNegotiatedContentResult < Movy >;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("Star Warts" , contentResult.Content.Title);
        }
        /// <summary>
        ///  test GetMovie returns notfound when the movie does not exist.
        /// </summary>
        [TestMethod]
        public void Should_Return_Not_Found_Movie()
        {
           
            // Act  
            IHttpActionResult actionResult = controller.GetMovie(100);
            // Assert  
            Assert.IsInstanceOfType(actionResult , typeof(NotFoundResult));
        }

        /// <summary>
        /// test GetMovieRecommendation returns recommendation with correct properties
        /// </summary>
        [TestMethod]
        public void Should_Return_Movie_Recommendation()
        {
            
            //Act
            var response = controller.GetMovieRecommendation(3);
            var contentResult = response as OkNegotiatedContentResult<MovieRecommendation>;

            // Assert  
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(3,contentResult.Content.ID);
            Assert.AreEqual("A night in Paris" , contentResult.Content.Title);
            Assert.IsTrue(contentResult.Content.recommendedMvs.Count == 5);
        }

        /// <summary>
        /// test GetMovieRecommendation for notfound if movie id does not exist
        /// </summary>
        [TestMethod]
        public void Should_Return_Not_Found_Recommendation()
        {

            //Act
            IHttpActionResult actionResult = controller.GetMovieRecommendation(1928);

            // Assert  
            Assert.IsInstanceOfType(actionResult , typeof(NotFoundResult));
        }

        /// <summary>
        /// test GetStrings if the the objects returned are of type string and their amount
        /// </summary>
        [TestMethod]
        public void Should_Return_Strings()
        {

            //Act
            var response = controller.GetStrings();

            //Assert
            Assert.IsInstanceOfType(response[1] , typeof(string));
            Assert.AreEqual(4, response.Count);
        }

        /// <summary>
        /// tests Calculate  for exceptions
        /// </summary>
        [TestMethod]
        public void Should_Throw_DivideBy0_Exception()
        {
            try
            {
                //Act
                var response = controller.Calculate(1 , 0);
                //Assert
                Assert.Fail();
            }
            catch ( DivideByZeroException e )
            {
               //correctly threw exception
            }
        }

        /// <summary>
        /// tests Calculate for correct division
        /// </summary>
        [TestMethod]
        public void Should_Divide_2_numbers()
        {
            //Act
            var response = controller.Calculate(10 , 5);
            //Assert
            int nr;
            Assert.AreEqual(int.Parse(response) , 2);
        }
    }
}
