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
        private List<Movy> movieDataSet;

        [TestInitialize]
        public void Initialize()
        {

             movieDataSet = new List<Movy>
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
        }

        /// <summary>
        /// test GetMovies returns a list of 13 movies where the first has the correct title
        /// </summary>
        [TestMethod]
        public void GetMovies_ShouldReturnListOfMovies()
        {
            ////Act
            //var response = controller.GetMovies();
            //Assert.IsNotNull(response);
            //var contentResult = response as IQueryable<Movy>;
            //var movieList = contentResult.ToList();
            ////Assert
            //Assert.IsNotNull(contentResult);
            //Assert.IsTrue(movieList.Count == 13);
            //Assert.AreEqual("Star Warts" , movieList[0].Title);

            //arrange
            var set = new Mock<DbSet<Movy>>().SetupData(movieDataSet);

            var context = new Mock<MoviesEntities>();
            context.Setup(c => c.Movies).Returns(set.Object);

            var controller = new MoviesController(context.Object);

            //act
            var result = controller.GetMovies().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3,result.Count);
        }
        /// <summary>
        /// test GetMovie returns the title of the movie object returned matches the one in the database at id = 1
        /// </summary>
        [TestMethod]
        public void GetMovie_ShouldReturnMovieWithId()
        {
            //var controller = new MoviesController(null);
            ////Act
            //var response = controller.GetMovie(1);
            //var contentResult = response as OkNegotiatedContentResult < Movy >;
            ////Assert
            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
            //Assert.AreEqual("Star Warts" , contentResult.Content.Title);

            //arrange
            var set = new Mock<DbSet<Movy>>().SetupData(movieDataSet);

            var context = new Mock<MoviesEntities>();
            context.Setup(c => c.Movies).Returns(set.Object);

            var controller = new MoviesController(context.Object);

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
            //var controller = new MoviesController(null);
            ////Act
            //var response = controller.GetMovie(1);
            //var contentResult = response as OkNegotiatedContentResult < Movy >;
            ////Assert
            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
            //Assert.AreEqual("Star Warts" , contentResult.Content.Title);

            //arrange
            var set = new Mock<DbSet<Movy>>().SetupData(movieDataSet);

            var context = new Mock<MoviesEntities>();
            context.Setup(c => c.Movies).Returns(set.Object);

            var controller = new MoviesController(context.Object);

            int testMovieId = 10;
            //act
            var result = controller.GetMovie(testMovieId);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        /// <summary>
        ///  test GetMovie returns notfound when the movie does not exist.
        /// </summary>
        [TestMethod]
        public void Should_Return_Not_Found_Movie()
        {
            var controller = new MoviesController(null);
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
            var controller = new MoviesController(null);
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
            var controller = new MoviesController(null);
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
            var controller = new MoviesController(null);
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

        /// <summary>
        /// tests Calculate for correct division
        /// </summary>
        [TestMethod]
        public void Should_Divide_2_numbers()
        {
            var controller = new MoviesController(null);
            //Act
            var response = controller.Calculate(10 , 5);
            //Assert
            int nr;
            Assert.AreEqual(int.Parse(response) , 2);
        }
    }
}
