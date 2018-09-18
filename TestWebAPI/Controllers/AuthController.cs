using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using TestWebAPI.Models;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/v1")]
    public class AuthController : ApiController
    {
        [Route("token")]
        public IHttpActionResult GenerateToken()
        {
            int userId = UserSecurity.GetUserId(Thread.CurrentPrincipal.Identity.Name);
            string token = Token.GenerateRandomNumber(20);
            Token.StoreToken(userId,token);
            AuthReturnObject response = new AuthReturnObject(token,DateTime.Now, DateTime.Now.AddHours(24),"token");
            return Ok(response);
        }
    }
}
