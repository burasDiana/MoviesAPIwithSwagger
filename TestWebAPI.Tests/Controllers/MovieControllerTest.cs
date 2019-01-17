using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestWebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using TestWebAPI.Models;
using System.Web.Http.Results;
using DataAccess;
using Moq;

namespace TestWebAPI.Tests.Controllers
{
    [TestClass]
    public class MovieControllerTest
    {
        private Mock<MoviesEntities> context;
        private Mock<DbSet<Movy>> set;
        private MoviesController controller;

        [TestInitialize]
        public void Initialize()
        {
            // setup movie data set to inject in the controller
             var movieDataSet = new List<Movy>
            {
                new Movy
                {
                    ID = 1, Genre = "Action", Price = 10, ReleaseDate = Convert.ToDateTime("2001-05-09"),
                    Title = "John Wick 2"
                },
                new Movy
                {
                    ID = 2, Genre = "Comedy", Price = 8, ReleaseDate = Convert.ToDateTime("2003-04-10"),
                    Title = "Deadpool"
                },
                new Movy
                {
                    ID = 2, Genre = "Drama", Price = 12, ReleaseDate = Convert.ToDateTime("2003-05-10"),
                    Title = "Snowstorm"
                }

            };

            set = new Mock<DbSet<Movy>>().SetupData(movieDataSet);

            context = new Mock<MoviesEntities>();
            context.Setup(c => c.Movies).Returns(set.Object);

            controller = new MoviesController(context.Object);
        }

        [TestMethod]
        public void GetMovies_ShouldReturnListOfMovies()
        {
            //act
            var result = controller.GetMovies().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3,result.Count);
        }
       
        [TestMethod]
        public void GetMovie_ShouldReturnMovieWithId()
        {
            //arrange
            int testMovieId = 1;

            //act
            var result = controller.GetMovie(testMovieId) as OkNegotiatedContentResult<Movy>;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testMovieId, result.Content.ID);
        }

        [TestMethod]
        public void GetMovie_ShouldReturnNotFound()
        {
            //arrange
            int testMovieId = 10;

            //act
            var result = controller.GetMovie(testMovieId);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void GetMovieRecommendation_ShouldReturnRecommendationForMovieId()
        {
            //arrange
            int movieTestId = 2;

            //Act
            var response = controller.GetMovieRecommendation(movieTestId);
            var contentResult = response as OkNegotiatedContentResult<MovieRecommendation>;

            // Assert  
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(movieTestId, contentResult.Content.ID);
            Assert.IsTrue(contentResult.Content.recommendedMvs.Count == 1); // recomment drama movie 1
        }

        [TestMethod]
        public void GetMovieRecommendation_ShouldReturnNotFound()
        {
            //arrange
            int recommendationId = 1988;

            //act
            var result = controller.GetMovieRecommendation(recommendationId);

            // Assert  
            Assert.IsInstanceOfType(result , typeof(NotFoundResult));
        }

       
        [TestMethod]
        public void GetStrings_ShouldReturnStrings()
        {
            //arrange
            var controller = new MoviesController(null);

            //Act
            var response = controller.GetStrings();

            //Assert
            Assert.IsInstanceOfType(response[1] , typeof(string));
            Assert.AreEqual(4, response.Count);
        }
        
        [TestMethod]
        public void Calculate_ShouldThrowDivideByZeroException()
        {
            //arrange
            var controller = new MoviesController(null);

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

        [TestMethod]
        public void Calculate_ShouldDivideNumbers()
        {
            //arrange
            var controller = new MoviesController(null);

            //Act
            var response = controller.Calculate(10 , 5);

            //Assert
            Assert.AreEqual(int.Parse(response) , 2);
        }

        [TestCleanup]
        public void RemoveDependencies()
        {
            controller = null;
            set = null;
            context = null;
        }
    }
}
