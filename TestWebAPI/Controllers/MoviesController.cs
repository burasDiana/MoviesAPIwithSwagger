using DataAccess;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TestWebAPI.Models;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{   
    [VersionFilter]
    [RoutePrefix("api/v1/movies")]
    public class MoviesController : ApiController
    {
        private List<string> list;
        //private MoviesModel db = new MoviesModel();
        private MoviesEntities db = new MoviesEntities();

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <remarks>
        /// This is a remark.
        /// </remarks>
        /// <returns>
        /// list of movies in the database
        /// </returns>
        // GET: api/Movies
        [CustomAuthentication(UserSecurity.UserType.Customer,UserSecurity.UserType.Admin)]
        //[Route("api/v1/movies")]
        [Route]
        [Queryable]
        [SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieExamples))]
        public IQueryable<Movy> GetMovies()
        {
            return db.Movies.AsQueryable();
        }

        //method for getting count of elements from Odata
        //[EnableQuery]
        //public IHttpActionResult GetMovies(ODataQueryOptions<Movy> queryOptions)
        //{
        //    var query = db.Movies.AsQueryable();
        //    var queryResults = (IQueryable<Movy>)queryOptions.ApplyTo(query);
        //    var cnt = Request.ODataProperties().TotalCount;
        //    return Ok(new PageResult<Movy>(queryResults, null, Request.ODataProperties().TotalCount));
        //}

        /// <summary>
        /// Returns some strings
        /// </summary>
        /// <returns></returns>
        //[Route("api/Movies/GetStrings")]
        //[Route("api/v1/movies/GetStrings")]
        [Route("GetStrings")]
        public List<string> GetStrings()
        {
            list = new List<string>() { "abc" , "123" , "456" , "butter" };
            return list;         
        }

        /// <summary>
        /// Returns some strings
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        //[Route("api/Movies/Calculate")]
        //[Route("api/v1/movies/Calculate")]
        [Route("Calculate")]
        public string Calculate(int a, int b)
        {
            return (a / b).ToString();
        }

        /// <summary>
        /// Returns current time
        /// </summary>
        /// <returns></returns>
        //[Route("api/Movies/GetTime")]
        [Route("GetTime")]
        public HttpResponseMessage GetTime()
        {
            string time = DateTime.Now.Hour + ":" +  DateTime.Now.Minute;
              return  Request.CreateResponse(HttpStatusCode.Accepted , time);
        }

        /// <summary>
        /// Get recommendations for a movie
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">Returns the created recommendation</response>
        /// <response code="400">If the movie corresponding to the id is null</response>
        // GET: api/Movies/GetRecommendation
        [CustomAuthentication]
        [SwaggerResponse(201, "Returns the created recommendation", typeof(MovieRecommendation))]
        [SwaggerResponse(400 , "If the movie corresponding to the id is null")]
        //[SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieRecommendation))]
        //[Route("api/Movies/GetMovieRecommendation")]
        [Route("GetMovieRecommendation")]
        public IHttpActionResult GetMovieRecommendation(int id)
        {
            var movie = db.Movies.FirstOrDefault(m => m.ID == id);
            if ( movie == null )
            {
                return NotFound();
            }

            Random rnd = new Random();
            var percentage = rnd.Next(0 , 50) * 2;
            var moviequery = from m in db.Movies
                             where m.Genre == "Drama"
                             select m;

            var rec = new MovieRecommendation();
            rec.ID = id;
            rec.Title = movie.Title;
            rec.matchScore = percentage + "%";
            rec.recommendedMvs = moviequery.ToList();

            return Ok(rec);
        }

        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Movies/5
        [CustomAuthentication]
        [ResponseType(typeof(Movie))]
        [SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieExamples))]
        [Route("{id}")]
        public IHttpActionResult GetMovie(int id)
        {
            using (MoviesEntities db = new MoviesEntities() )
            {
                var movie = db.Movies.FirstOrDefault(m => m.ID == id);
                if ( movie == null )
                {
                    return NotFound();
                }

                return Ok(movie);
            }
            
        }

        /// <summary>
        /// Edit a movie with id = id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        // PUT: api/Movies/5
        [CustomAuthentication]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovie(int id, Movy movie)
        {
            using ( MoviesEntities db = new MoviesEntities() )
            {
                if ( !ModelState.IsValid )
                {
                    return BadRequest(ModelState);
                }

                if ( id != movie.ID )
                {
                    return BadRequest();
                }

                db.Entry(movie).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !MovieExists(id) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Edit a movie with id = id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        // PUT: api/Movies/5
        [CustomAuthentication(UserSecurity.UserType.Customer)]
        [ResponseType(typeof(void))]
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult PatchMovie(int id, MoviePatchRequest request)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }

            var movie = db.Movies.FirstOrDefault(m => m.ID == id);

            movie.Price = request.Price.HasValue ? request.Price.Value : movie.Price;

            db.SaveChanges();

            return Ok(movie);
        }

        /// <summary>
        /// Add a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        // POST: api/Movies
        [CustomAuthentication]
        [ResponseType(typeof(Movy))]
        [SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieExamples))]
        public IHttpActionResult PostMovie(Movy movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.ID }, movie);
        }

        /// <summary>
        /// Delete a movie with id = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Movies/5
        [CustomAuthentication]
        [ResponseType(typeof(Movy))]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movy movie = db.Movies.FirstOrDefault(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            using ( MoviesEntities db = new MoviesEntities() )
            {
                return db.Movies.Count(e => e.ID == id) > 0;
            }
        }
    }
}