﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TestWebAPI.Controllers
{
    // This class is used to process custom error cases
    public class ErrorController : ApiController
    {
        /// <summary>
        /// This method is used for handling requests to invalid urls
        /// </summary>
        [HttpGet, HttpPost, HttpPut, HttpDelete, AcceptVerbs("PATCH")]
        public IHttpActionResult Handle404()
        {
            string url = HttpContext.Current.Request.RawUrl;
            if (url.Contains("movies"))
            {
                return Content(HttpStatusCode.NotFound, "Did you mean 'api/v1/movies'?");
            }
            else if(url.Contains("users"))
            {
                return Content(HttpStatusCode.NotFound, "Did you mean 'api/v2/users'?");
            }

            return Content(HttpStatusCode.NotFound, "The requested resource is not found.");
        }
    }
}
