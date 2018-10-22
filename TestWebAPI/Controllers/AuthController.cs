using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using DataAccess;
using Microsoft.Data.OData.Query.SemanticAst;
using TestWebAPI.Models;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{
    [VersionFilter]
    [RoutePrefix("api/v1")]
    public class AuthController : ApiController
    {
        [Route("token")]
        
        [CustomAuthentication]
        public IHttpActionResult GenerateToken()
        {
            int userId = UserSecurity.GetUserId(Thread.CurrentPrincipal.Identity.Name);
            string token = Token.GenerateRandomNumber(20);
            Token.StoreToken(userId,token);
            AuthReturnObject response = new AuthReturnObject(token,DateTime.Now, DateTime.Now.AddHours(24),"Token", "84900");
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("test")]
        //[Queryable]
        //public IQueryable<Teacher> Get()
        //{
        //    List<Teacher> teachers = new List<Teacher>();
        //    Teacher teacher = new Teacher()
        //    {
        //        Id = 1897,
        //        SuperId = "k39c0smf=cm}",
        //        Email = "teach@email.com",
        //        FullName = "James Martin",
        //        ImageUrl = "imageURL",
        //        Phone = "919637918",
        //        Students = new List<Student>()
        //        {
        //            new Student{Id = 1,ClassId = "12A",Email = "james@james.com",FullName = "James Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "32939233-12"},
        //            new Student{Id = 2,ClassId = "12A",Email = "alex@alex.com",FullName = "Alex Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "329345233-12"}
        //        }
        //    };
        //    teachers.Add(teacher);
        //    return teachers.AsQueryable();
        //}

        // query example : http://localhost:53562/api/v1/test?$expand=Students&$select=FullName,Id,Students/FullName
        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        [Queryable]
        public IQueryable<Teacher> Get()
        {
            List<Teacher> teachers = new List<Teacher>();
            Teacher teacher = new Teacher()
            {
                Id = 1897,
                SuperId = "k39c0smf=cm}",
                Email = "teach@email.com",
                FullName = "James Martin",
                ImageUrl = "imageURL",
                Phone = "919637918",
                Students = new List<Student>()
                {
                    new Student{Id = 1,ClassId = "12A",Email = "james@james.com",FullName = "James Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "32939233-12"},
                    new Student{Id = 2,ClassId = "12A",Email = "alex@alex.com",FullName = "Alex Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "329345233-12"}
                }
            };
            Teacher teacher1 = new Teacher()
            {
                Id = 1898,
                SuperId = "2333RG-r54",
                Email = "teach1@email.com",
                FullName = "Robert Cross",
                ImageUrl = "imageURL",
                Phone = "919635918",
                Students = new List<Student>()
                {
                    new Student{Id = 1,ClassId = "12A",Email = "james@james.com",FullName = "James Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "32939233-12"},
                    new Student{Id = 2,ClassId = "12A",Email = "alex@alex.com",FullName = "Alex Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "329345233-12"},
                    new Student{Id = 3,ClassId = "12A",Email = "alex@alex.com",FullName = "Alex Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "329345233-12"},
                    new Student{Id = 4,ClassId = "12A",Email = "alex@alex.com",FullName = "Alex Waters",ImageUrl = "imageURL",ParentPhoneNr = "91283043-32",Phone = "329345233-12"}
                }
            };
            teachers.Add(teacher);
            teachers.Add(teacher1);
            return teachers.AsQueryable();
        }

    }
}
