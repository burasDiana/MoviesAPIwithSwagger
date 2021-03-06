﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Http;
using DataAccess;
using Newtonsoft.Json.Linq;
using TestWebAPI.Models;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{
    /// <summary>
    ///  This class handler operations related to authentication, like token generation and password changing
    /// </summary>
    [VersionFilter]
    [RoutePrefix("api/v1")]
    public class AuthController : ApiController
    {
        // code for reseting a password - temporary - only for development
        private static string secretResetCode = "2Gh29F";
        private static string secretVerifCode = "my_token_verify";

        [AllowAnonymous]
        [Route("fbwebhook")]
        [HttpPost]
        [HttpGet]
        public IHttpActionResult FacebookWebhook()
        {
            // verify webhook
            var challenge = HttpContext.Current.Request.Params.Get("hub.challenge");

            var token = HttpContext.Current.Request.Params.Get("hub.verify_token");

            if (token != null && token == secretVerifCode)
            {
                return Ok(challenge);
            }

            //receive data dump
            string input = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            var jsonParsed = JObject.Parse(input);
            var leadInfo = jsonParsed["entry"][0]["changes"][0]["value"];

            if (leadInfo == null)
            {
                FacebookManager.SendMailToAdmins("Lead info was null.", "Lead data error");
                return Ok(); // send a 200 OK to facebook so they do not send the request over and over
            }
            if (leadInfo["leadgen_id"] == null)
            {
                FacebookManager.SendMailToAdmins("Leadgen_id was null.", "Lead data error");
                return Ok();
            }

            var leadId = leadInfo["leadgen_id"].ToString();
            // var formId = leadInfo["form_id"].ToString();
            // var adId = leadInfo["ad_id"] != null ? leadInfo["ad_id"].ToString() : " ";
            // var adGroupId = leadInfo["adgroup_id"] != null ? leadInfo["adgroup_id"].ToString() : " ";


            if (FacebookManager.GetPageAccessToken())
            {
                // token verification passed, continue to get info about lead from graph api
                //process lead data in the system
                FacebookManager.ProcessLead(leadId);
                return Ok();
            }

            return BadRequest(); // make facebook resend the request
        }

        /// <summary>
        ///  Create, store and return a user token
        /// </summary>
        [Route("token")]
        [CustomAuthentication]
        public IHttpActionResult GenerateToken()
        {
            int userId = UserSecurity.GetUserId(Thread.CurrentPrincipal.Identity.Name);
            string token = TokenHandler.GenerateRandomNumber(20);
            TokenHandler.StoreToken(userId, token);
            AuthReturnObject response = new AuthReturnObject(token, DateTime.Now, DateTime.Now.AddHours(24), "TokenHandler", "84900");
            return Ok(response);
        }

        /// <summary>
        /// Allows user to request a password reset when they forgot their password
        /// </summary>
        [AllowAnonymous]
        [Route("requestPasswordReset")]
        public IHttpActionResult RequestPasswordReset(ResetPasswordRequest request)
        {
            if (UserSecurity.FindUser(request.UserName))
            {
                if (request.EmailAddress != string.Empty && EmailAddressValid(request.EmailAddress))
                {
                    SendMailToAddress(request.EmailAddress, secretResetCode);
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, "Email address invalid.");
                }
                return Ok();
            }
            return Content(HttpStatusCode.Unauthorized, "User not found.");
        }

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        [AllowAnonymous]
        [Route("resetPassword")]
        public IHttpActionResult ResetPassword(ResetPasswordRequest request)
        {
            int userId = UserSecurity.GetUserId(request.UserName);

            if (request.SecretCode != null && request.SecretCode == secretResetCode)
            {
                ResetUserPassword(request.NewPassword, userId);
                return Ok();
            }
            else if (UserSecurity.Login(request.UserName, request.OldPassword))
            {
                //password matches user
                ResetUserPassword(request.NewPassword, userId);
                return Ok();
            }
            return Content(HttpStatusCode.Unauthorized, "Password or secret code does not match.");
        }

        /// <summary>
        /// This is a test method. To be removed 
        /// </summary>
        // query example : http://localhost:53562/api/v1/test?$expand=Students&$select=FullName,Id,Students/FullName
        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        //[Queryable]
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

        /// <summary>
        /// This method is used to send a password reset code to a user by email.
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="resetCode"></param>
        private void SendMailToAddress(string mailAddress, string resetCode)
        {
            MailMessage mail = new MailMessage("test@testwebapi.com", mailAddress);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            mail.Subject = "Reset password secret code";
            mail.Body = "Use the follwing code to reset your password: " + resetCode;
            client.Send(mail);
        }

        /// <summary>
        /// This method resets a user's password in the database
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userId"></param>
        private void ResetUserPassword(string password, int userId)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                db.Users.FirstOrDefault(u => u.Id.Equals(userId)).Password = password;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method checks that the email address is valid
        /// </summary>
        private bool EmailAddressValid(string email)
        {
            if (email.Contains('@') && (email.Contains(".com") || email.Contains("hotmail") || email.Contains("me") || email.Contains("gmail") || email.Contains(".")))
            {
                return true;
            }
            return false;
        }
    }
}
