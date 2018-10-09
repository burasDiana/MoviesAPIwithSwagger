using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;

namespace TestWebAPI.Controllers
{
    [RoutePrefix("api/v2/users")]
    public class UsersController : ApiController
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<User> Get()
        //{
        //    using ( MoviesEntities entities = new MoviesEntities() )
        //    {
        //        return entities.Users.ToList();
        //    }
        //}
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public User Get(int id)
        //{
        //    using ( MoviesEntities entities = new MoviesEntities() )
        //    {
        //        return entities.Users.FirstOrDefault(u => u.Id == id);
        //    }
        //}
        [Route("")]
        public IHttpActionResult Get()
        {
            using ( MoviesEntities entities = new MoviesEntities() )
            {
                return Ok(entities.Users.ToList());
            }

        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            using ( MoviesEntities entities = new MoviesEntities() )
            {
                var user = entities.Users.FirstOrDefault(u => u.Id == id);
                if ( user == null )
                {
                    return Content(HttpStatusCode.NotFound , "User not found");
                }
                else
                {
                    return Ok(user);
                }
            }
        }
    }
}
