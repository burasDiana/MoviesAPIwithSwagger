using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;
using TestWebAPI.Models;

namespace TestWebAPI.Controllers
{
    [RoutePrefix("api/v2/users")]
    public class UsersController : ApiController
    {

        private MoviesEntities db = new MoviesEntities();

        /// <summary>
        /// This method returns a list of users, or alternatively if a movieId is provided, that specific movie
        /// </summary>
        [Route("{id:int?}")]
        public IHttpActionResult Get(int? movieId = null)
        {
            if (movieId.HasValue)
            {
                var users1 = (from u in db.Users
                              where u.MovieId == movieId
                    select new UserModel
                    {
                        Email = u.Email,
                        Id = u.Id,
                        Username = u.Username,
                        MovieId = u.MovieId
                    }).ToList();
                return Ok(users1);
            }

            var users = (from u in db.Users
                         select new UserModel
                         {
                             Email = u.Email,
                             Id = u.Id,
                             Username = u.Username,
                             MovieId = u.MovieId
                         }).ToList();

            return Ok(users);
        }

        /// <summary>
        /// This method returns a movie by id
        /// </summary>
        [Route("GetUnique/{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found");
            }
            else
            {
                return Ok(new UserModel()
                {
                    Email = user.Email,
                    Id = user.Id,
                    Username = user.Username,
                    MovieId = user.MovieId
                });
            }
        }

        
        /// <summary>
        /// This method adds a new user to the database
        /// </summary>
        [Route("")]
        public IHttpActionResult Post([FromBody] UserModel user) //[FromBody]
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(new User(){Email = user.Email , Username = user.Username, MovieId = user.MovieId,Password = user.Password});
                db.SaveChanges();

                var createdUser = db.Users.FirstOrDefault(u => u.Username == user.Username);

                return Ok(new UserModel(){Email = createdUser.Email, Username =  createdUser.Username, MovieId = createdUser.MovieId, Id = createdUser.Id,Password =  createdUser.Password});
            }

            return BadRequest();

        }

        /// <summary>
        /// This method edits a user's username in the database
        /// </summary>
        [Route("")]
        public IHttpActionResult Put(int id, string userName) //[FromBody]
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found");
            }
            else
            {
                user.Username = userName;
                db.SaveChanges();
                return Ok();
            }
            
        }

        /// <summary>
        /// This method deletes a user from the database
        /// </summary>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found");
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return Ok();
        }
    }
}
