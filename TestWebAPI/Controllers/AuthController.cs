using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using DataAccess;
using TestWebAPI.Models;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{
    
    [RoutePrefix("api/v1")]
    public class AuthController : ApiController
    {
        [BasicAuthentication]
        [Route("token")]
        public IHttpActionResult GenerateToken()
        {
            int userId = UserSecurity.GetUserId(Thread.CurrentPrincipal.Identity.Name);
            string token = Token.GenerateRandomNumber(20);
            Token.StoreToken(userId,token);
            AuthReturnObject response = new AuthReturnObject(token,DateTime.Now, DateTime.Now.AddHours(24),"Token");
            return Ok(response);
        }

        [AllowAnonymous]
        [Route("resetPassword")]
        public IHttpActionResult ResetPassword(ResetPasswordRequest request)
        {
            if (UserSecurity.Login(request.UserName, request.OldPassword))
            {
                int userId = UserSecurity.GetUserId(request.UserName);
                //password matches user
                using (MoviesEntities db = new MoviesEntities())
                {
                    db.Users.FirstOrDefault(u => u.Id.Equals(userId)).Password = request.NewPassword;
                    db.SaveChanges();
                }

                return Ok();
            }
                return Content(HttpStatusCode.Unauthorized, "password or user name incorrect");
        }
    }
}
