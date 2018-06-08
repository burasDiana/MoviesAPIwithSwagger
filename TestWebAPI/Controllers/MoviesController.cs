using DataAccess;
using Swashbuckle.Examples;
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
        [BasicAuthentication]
        [SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieExamples))]
        public IQueryable<Movy> GetMovies()
        {
            return db.Movies;
        }

        /// <summary>
        /// Returns some strings
        /// </summary>
        /// <returns></returns>
        [Route("api/movies/GetStrings")]
        public List<string> GetStrings()
        {
            list = new List<string>() { "abc" , "123" , "456" , "butter" };
            return list;         
        }

        /// <summary>
        /// Returns current time
        /// </summary>
        /// <returns></returns>
        [Route("api/movies/GetTime")]
        public HttpResponseMessage GetTime()
        {
            string time = DateTime.Now.Hour + ":" +  DateTime.Now.Minute;
              return  Request.CreateResponse(HttpStatusCode.Accepted , time);
        }

        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Movies/5
        [BasicAuthentication]
        [ResponseType(typeof(Movie))]
        [SwaggerResponseExample(HttpStatusCode.OK , typeof(MovieExamples))]
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
        [BasicAuthentication]
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
        /// Add a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        // POST: api/Movies
        [BasicAuthentication]
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
        [BasicAuthentication]
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